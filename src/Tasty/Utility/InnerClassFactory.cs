
namespace Tasty.Utility
{
    public class InnerClassFactory<OuterType> where OuterType : class
    {
        public InnerClassFactory(OuterType outer)
        {
            _outer = outer;
        }

        private readonly OuterType _outer;

        public InnerClassType NestedNew<InnerClassType>()
            where InnerClassType : IInnerClass<OuterType>, new()
        {
            return Nested(new InnerClassType());
        }

        public InnerClassType Nested<InnerClassType>(InnerClassType newInstance)
            where InnerClassType : IInnerClass<OuterType>
        {
            ((IOuterSetter<OuterType>)newInstance).Outer = _outer;
            return newInstance;
        }
    }
}
