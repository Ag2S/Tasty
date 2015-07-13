using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tasty.MockServer
{
    public class MockServerProviderCollection : IEnumerable<IMockServerProvider>
    {
        private readonly IDictionary<Type, IMockServerProvider> _serverProviderDictionary = new Dictionary<Type, IMockServerProvider>();

        public bool Contains<T>() where T : IMockServerProvider
        {
            return _serverProviderDictionary.ContainsKey(typeof(T));
        }

        public T Of<T>() where T : IMockServerProvider, new()
        {
            if (!Contains<T>())
                Add(new T());
            return (T)this[typeof(T)];
        }

        public IMockServerProvider this[Type type]
        {
            get
            {
                return _serverProviderDictionary[type];
            }
        }

        public void Add<T>(T serverProvider) where T : IMockServerProvider
        {
            _serverProviderDictionary.Add(typeof(T), serverProvider);
        }

        public IEnumerator<IMockServerProvider> GetEnumerator()
        {
            return _serverProviderDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public void Start()
        {
            foreach (var serverProvider in _serverProviderDictionary.Values)
            {
                serverProvider.Start();
            }
        }

        public void Stop()
        {
            foreach (var serverProvider in _serverProviderDictionary.Values.Reverse())
            {
                serverProvider.Stop();
            }
        }
    }
}
