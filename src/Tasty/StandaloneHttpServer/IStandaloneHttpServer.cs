using System;

namespace Tasty.StandaloneHttpServer
{
    public interface IStandaloneHttpServer : IDisposable
    {
        void Start(short port, string physicalPath);
        void Stop();
    }
}
