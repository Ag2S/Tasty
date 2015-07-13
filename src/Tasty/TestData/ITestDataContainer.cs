
namespace Tasty.TestData
{
    public interface ITestDataContainer
    {
        IValueAccessor<T> Of<T>(string name);
        IValueAccessor<T> Of<T>(int serialNumber = 1);
    }
}
