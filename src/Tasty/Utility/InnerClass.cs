
namespace Tasty.Utility
{
    public class InnerClass<OuterType> : IInnerClass<OuterType> where OuterType : class
    {
        public OuterType Outer { get; private set; }

        OuterType IOuterSetter<OuterType>.Outer
        {
            set
            {
                Outer = value;
            }
        }
    }
}
