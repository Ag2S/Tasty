using System;

namespace Tasty.Mock
{
    public interface IMockFactory
    {
        object CreateMock(Type type);
    }
}
