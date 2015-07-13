using Tasty.Database;
using Tasty.MockServer;
using Tasty.StandaloneHttpServer;

namespace Tasty
{
    public abstract class WebsiteIntegrationTests : Tests
    {
        protected MockServerProviderCollection MockServers { get; private set; }
        protected IDatabaseRestorer DatabaseRestorer { get; private set; }
        protected IStandaloneHttpServer StandaloneHttpServer { get; private set; }

        protected abstract IDatabaseRestorer CreateDatabaseRestorer();

        protected override void InitializeTest()
        {
            base.InitializeTest();
            MockServers = new MockServerProviderCollection();
            DatabaseRestorer = CreateDatabaseRestorer();
            StandaloneHttpServer = new IISExpressServer();
        }

        protected override void ArrangeDefault()
        {
            DatabaseRestorer.CreateSnapshot();
        }

        protected override void CleanUpTest()
        {
            StandaloneHttpServer.Dispose();
            DatabaseRestorer.Restore();
            MockServers.Stop();
            base.CleanUpTest();
        }
    }
}
