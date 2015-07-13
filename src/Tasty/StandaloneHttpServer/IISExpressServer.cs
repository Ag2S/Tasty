using System;
using System.Diagnostics;

namespace Tasty.StandaloneHttpServer
{
    public class IISExpressServer : IStandaloneHttpServer
    {
        const string IISEXPRESS_PATH = @"C:\Program Files\IIS Express\iisexpress.exe";
        private readonly Process _iisExpressProcess = new Process();

        public void Start(short port, string physicalPath)
        {
            _iisExpressProcess.StartInfo = new ProcessStartInfo
            {
                FileName = IISEXPRESS_PATH,
                Arguments = string.Format(@"/path:{0} /port:{1}", physicalPath, port),
                UseShellExecute = false
            };
            _iisExpressProcess.Start();
        }

        public void Stop()
        {
            _iisExpressProcess.Kill();
            _iisExpressProcess.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
