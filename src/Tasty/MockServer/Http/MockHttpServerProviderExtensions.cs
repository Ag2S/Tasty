using Tasty.MockServer;
using Tasty.MockServer.Http;

namespace Tasty
{
    public static class MockHttpServerProviderExtensions
    {
        public static MockHttpServerProvider OfHttp(this MockServerProviderCollection servers)
        {
            return servers.Of<MockHttpServerProvider>();
        }
    }
}
