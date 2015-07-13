
namespace Tasty.Utility
{
    public static class InnerClassExtensions
    {
        public static InnerClassFactory<T> Create<T>(this T instance)
            where T : class
        {
            return new InnerClassFactory<T>(instance);
        }
    }
}
