using System;

namespace Tasty.Mock
{
    public interface IMockProvider
    {
        T Of<T>(string name) where T : class;
        T Of<T>(int serialNumber = 1) where T : class;
        object Of(Type type, string name);
        object Of(Type type, int serialNumber = 1);
        void Add(Type type, string name, object mock);
        void Add(Type type, int serialNumber, object mock);
        void Add(Type type, object mock);
        void Add<T>(T mock, int serialNumber = 1);
        void Add<T>(T mock, string name);
    }
}
