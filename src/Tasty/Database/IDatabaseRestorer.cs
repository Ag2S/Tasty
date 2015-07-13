
namespace Tasty.Database
{
    public interface IDatabaseRestorer
    {
        void CreateSnapshot();
        void Restore();
    }
}
