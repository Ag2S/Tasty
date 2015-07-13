using System;
using System.Net;
using System.Threading.Tasks;
using Tasty.MockServer.Configure;

namespace Tasty.MockServer.Http
{
    public class MockHttpServerProvider : IMockServerProvider
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly ConfigureCollection<HttpListenerRequest, HttpListenerResponse> _configure =
            new ConfigureCollection<HttpListenerRequest,HttpListenerResponse>(
                response => {
                    response.StatusCode = 404;
                });

        private void Listen()
        {
            _listener.BeginGetContext(
                asyncResult =>
                {
                    if (_listener.IsListening)
                        Task.Factory.StartNew(Listen);

                    try
                    {
                        var context = _listener.EndGetContext(asyncResult);
                        var fillUpResponse = _configure.GetResponderFor(context.Request);
                        fillUpResponse(context.Response);
                        context.Response.Close();
                    }
                    catch(HttpListenerException)
                    {
                        return;
                    }
                },
                null);
        }

        private class Configurator : AbstractConfigurator<MockHttpServerProvider, HttpListenerRequest, HttpListenerResponse>
        {
            private readonly string _baseUrl;

            public Configurator(MockHttpServerProvider target, string baseUrl)
                : base(target)
            {
                _baseUrl = baseUrl;
            }

            protected override void ApplyConfigure()
            {
                _target._listener.Prefixes.Add(_baseUrl);
                _target._configure.Add(_currentConfigure);
            }
        }

        public IWhenClause<HttpListenerRequest, HttpListenerResponse> At(string baseUrl)
        {
            return new Configurator(this, baseUrl);
        }

        public void Start()
        {
            if (_listener.IsListening) return;
            _listener.Start();
            Task.Factory.StartNew(Listen);
        }

        public void Stop()
        {
            if (!_listener.IsListening) return;
            _listener.Stop();
        }
    }
}
