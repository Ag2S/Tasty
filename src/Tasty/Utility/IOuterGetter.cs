
namespace Tasty.Utility
{
    public interface IOuterGetter<T> where T : class
    {
        T Outer { get; }
    }
}
