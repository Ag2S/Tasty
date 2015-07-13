using Tasty.MockServer;
using Tasty.MockServer.Smtp;

namespace Tasty
{
    public static class MockSmtpServerExtensions
    {
        public static MockSmtpServer OfSmtp(this MockServerProviderCollection servers)
        {
            return servers.Of<MockSmtpServer>();
        }

        public static void EnableSmtp(this MockServerProviderCollection servers)
        {
            servers.Add(new MockSmtpServer());
        }
    }
}
