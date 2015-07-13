using System;
using System.Collections.Generic;
using Tasty.Utility;

namespace Tasty.Mock
{
    public class MockProvider : IMockProvider
    {
        public MockProvider(IMockFactory factory)
        {
            _factory = factory;
        }

        private readonly IMockFactory _factory;

        public T Of<T>(string name) where T : class
        {
            return (T)Of(typeof(T), TypeDictionaryKey.From(typeof(T), name));
        }

        public T Of<T>(int serialNumber = 1) where T : class
        {
            return (T)Of(typeof(T), TypeDictionaryKey.From(typeof(T), serialNumber));
        }

        public object Of(Type type, string name)
        {
            return Of(type, TypeDictionaryKey.From(type, name));
        }

        public object Of(Type type, int serialNumber = 1)
        {
            return Of(type, TypeDictionaryKey.From(type, serialNumber));
        }

        private object Of(Type type, TypeDictionaryKey key)
        {
            object value;
            bool hasFound = _mockDictionary.TryGetValue(key, out value);

            if (!hasFound)
            {
                value = _factory.CreateMock(type);
                _mockDictionary.Add(key, value);
            }

            return value;
        }

        public void Add(Type type, string name, object mock)
        {
            if (!type.IsAssignableFrom(mock.GetType()))
                throw new ArgumentException("Type mismatch.", "mock");
            _mockDictionary.Add(TypeDictionaryKey.From(type, name), mock);
        }

        public void Add(Type type, int serialNumber, object mock)
        {
            if (!type.IsAssignableFrom(mock.GetType()))
                throw new ArgumentException("Type mismatch.", "mock");
            _mockDictionary.Add(TypeDictionaryKey.From(type, serialNumber), mock);
        }

        public void Add(Type type, object mock)
        {
            Add(type, 1, mock);
        }

        public void Add<T>(T mock, int serialNumber = 1)
        {
            _mockDictionary.Add(TypeDictionaryKey.From(typeof(T), serialNumber), mock);
        }

        public void Add<T>(T mock, string name)
        {
            _mockDictionary.Add(TypeDictionaryKey.From(typeof(T), name), mock);
        }

        private readonly IDictionary<TypeDictionaryKey, object> _mockDictionary = new Dictionary<TypeDictionaryKey, object>();
    }
}
