using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazored.LocalStorage.Testing
{
    internal class InMemoryStorageProvider : IStorageProvider
    {
        private readonly Dictionary<string, string> _dataStore = new Dictionary<string, string>();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public ValueTask ClearAsync()
        {
            _dataStore.Clear();
            return new ValueTask(Task.CompletedTask);
        }

        public bool ContainKey(string key)
            => _dataStore.ContainsKey(key);

        public ValueTask<bool> ContainKeyAsync(string key)
            => new ValueTask<bool>(ContainKey(key));

        public string GetItem(string key) 
            => _dataStore.ContainsKey(key) ? _dataStore[key] : default;

        public ValueTask<string> GetItemAsync(string key)
            => new ValueTask<string>(GetItem(key));

        public string Key(int index)
            => _dataStore.ElementAt(index).Key;

        public ValueTask<string> KeyAsync(int index)
            => new ValueTask<string>(Key(index));

        public int Length()
            => _dataStore.Count();

        public ValueTask<int> LengthAsync()
            => new ValueTask<int>(Length());

        public void RemoveItem(string key)
            => _dataStore.Remove(key);

        public ValueTask RemoveItemAsync(string key)
        {
            RemoveItem(key);
            return new ValueTask(Task.CompletedTask);
        }

        public void SetItem(string key, string data)
        {
            if (_dataStore.ContainsKey(key))
            {
                _dataStore[key] = data;
            }
            else
            {
                _dataStore.Add(key, data);
            }
        }

        public ValueTask SetItemAsync(string key, string data)
        {
            SetItem(key, data);
            return new ValueTask(Task.CompletedTask);
        }
    }
}
