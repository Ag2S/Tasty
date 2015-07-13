
namespace Tasty.Utility
{
    public interface IInnerClass<T> : IOuterGetter<T>, IOuterSetter<T> where T : class
    {
    }
}
