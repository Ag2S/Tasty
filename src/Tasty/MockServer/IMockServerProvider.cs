using System;

namespace Tasty.MockServer
{
    public interface IMockServerProvider
    {
        void Start();
        void Stop();
    }
}
