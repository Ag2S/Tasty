using Tasty.Mock;

namespace Tasty
{
    public abstract class UnitTests : Tests
    {
        protected IMockProvider Mock { get; private set; }

        protected abstract IMockFactory CreateMockFactory();

        protected override void InitializeTest()
        {
            base.InitializeTest();
            Mock = new MockProvider(CreateMockFactory());
        }
    }
}
