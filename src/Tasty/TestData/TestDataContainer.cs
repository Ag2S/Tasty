using Tasty.Utility;
using System.Collections.Generic;

namespace Tasty.TestData
{
    public class TestDataContainer : ITestDataContainer
    {
        public IValueAccessor<T> Of<T>(string name)
        {
            return Of<T>(TypeDictionaryKey.From(typeof(T), name));
        }

        public IValueAccessor<T> Of<T>(int serialNumber = 1)
        {
            return Of<T>(TypeDictionaryKey.From(typeof(T), serialNumber));
        }

        private IValueAccessor<T> Of<T>(TypeDictionaryKey key)
        {
            object value;
            bool hasFound = _testDataDictionary.TryGetValue(key, out value);

            if (!hasFound)
            {
                value = new ValueHolder<T>();
                _testDataDictionary.Add(key, value);
            }

            return (IValueAccessor<T>)value;
        }

        private class ValueHolder<T> : IValueAccessor<T>
        {
            public T Value { get; set; }
        }

        private readonly IDictionary<TypeDictionaryKey, object> _testDataDictionary = new Dictionary<TypeDictionaryKey, object>();
    }
}
