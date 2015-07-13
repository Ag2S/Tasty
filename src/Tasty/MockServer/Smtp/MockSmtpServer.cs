using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tasty.Utility;

namespace Tasty.MockServer.Smtp
{
    public class MockSmtpServer : IMockServerProvider
    {
        const int SMTP_PORT = 25;
        public MockSmtpServer()
        {
            _listener = new TcpListener(IPAddress.Any, SMTP_PORT);
            _mails = new ConcurrentBag<MailMessage>();
            _isListening = false;
        }

        private readonly TcpListener _listener;

        private readonly ConcurrentBag<MailMessage> _mails;
        private volatile bool _isListening;
        public IEnumerable<MailMessage> Mails { get { return _mails; } }

        private void Listen()
        {
            _listener.BeginAcceptTcpClient(
                asyncResult =>
                {
                    if (_isListening)
                        Task.Factory.StartNew(Listen);

                    try
                    {
                        var client = _listener.EndAcceptTcpClient(asyncResult);

                        var session = this.Create().Nested(new Session(client));
                        session.Process();
                    }
                    catch
                    {
                        return;
                    }
                },
                null);
        }

        public void Start()
        {
            if (_isListening) return;
            _listener.Start();
            Task.Factory.StartNew(Listen);
            _isListening = true;
        }

        public void Stop()
        {
            if (!_isListening) return;
            _listener.Stop();
            _isListening = false;
        }

        public class Session : InnerClass<MockSmtpServer>, IDisposable
        {
            private readonly TcpClient _client;
            private readonly StreamReader _reader;
            private readonly StreamWriter _writer;
            private MailMessage _currentMail;

            public Session(TcpClient client)
            {
                _client = client;
                _reader = new StreamReader(_client.GetStream());
                _writer = new StreamWriter(_client.GetStream());
                _writer.NewLine = "\r\n";
                _writer.AutoFlush = true;
                _currentMail = null;
            }

            private void AddMail(MailMessage mail)
            {
                Outer._mails.Add(mail);
            }

            public void Process()
            {
                try
                {
                    _writer.WriteLine("220 localhost -- Tasty mock smtp server");
                    string line;
                    while ((line = _reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("HELO"))
                        {
                            _writer.WriteLine("250 OK");
                        }
                        else if (line.StartsWith("EHLO"))
                        {
                            _writer.WriteLine("250 OK");
                        }
                        else if (line.StartsWith("MAIL FROM"))
                        {
                            _currentMail = new MailMessage();
                            _currentMail.HeadersEncoding = Encoding.UTF8;
                            _currentMail.BodyEncoding = Encoding.UTF8;
                            _currentMail.SubjectEncoding = Encoding.UTF8;
                            var from = Regex.Match(line.Remove(0, 10), @"<(.*)>").Groups[1].Value;
                            _currentMail.From = new MailAddress(from);
                            _writer.WriteLine("250 OK");
                        }
                        else if (line.StartsWith("RCPT TO"))
                        {
                            var to = Regex.Match(line.Remove(0, 8), @"<(.*)>").Groups[1].Value;
                            _currentMail.To.Add(new MailAddress(to));
                            _writer.WriteLine("250 OK");
                        }
                        else if (line.StartsWith("DATA"))
                        {
                            _writer.WriteLine("354 Start mail input; end with <CR><LF>.<CR><LF>");
                            while ((line = _reader.ReadLine()) != string.Empty)
                            {
                                var match = Regex.Match(line, @"(?<Name>.*):\s(?<Value>.*)");
                                var name = match.Groups["Name"].Value;
                                var value = match.Groups["Value"].Value;
                                _currentMail.Headers.Add(name, value);
                                if (name == "Content-Transfer-Encoding" && value == "base64")
                                    _currentMail.BodyTransferEncoding = TransferEncoding.Base64;
                            }
                            var bodyStringBuilder = new StringBuilder();
                            while ((line = _reader.ReadLine()) != ".")
                            {
                                bodyStringBuilder.AppendLine(line);
                            }
                            if (_currentMail.BodyTransferEncoding == TransferEncoding.Base64)
                                _currentMail.Body = _currentMail.BodyEncoding.GetString(Convert.FromBase64String(bodyStringBuilder.ToString()));
                            AddMail(_currentMail);
                            _currentMail = null;
                            _writer.WriteLine("250 OK");
                        }
                        else if (line.StartsWith("QUIT"))
                        {
                            _writer.WriteLine("221 Bye");
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    Dispose();
                }
            }

            public void Dispose()
            {
                _reader.Dispose();
                _writer.Dispose();
                _client.Close();
            }
        }
    }
}
