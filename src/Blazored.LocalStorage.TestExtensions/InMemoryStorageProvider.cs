using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage.TestExtensions
{
    internal class InMemoryStorageProvider : IStorageProvider
    {
        private readonly Dictionary<string, string> _dataStore = new Dictionary<string, string>();

        public void Clear()
            => _dataStore.Clear();

        public ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            _dataStore.Clear();
            return new ValueTask(Task.CompletedTask);
        }

        public bool ContainKey(string key)
            => _dataStore.ContainsKey(key);

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
            => new ValueTask<bool>(ContainKey(key));

        public string GetItem(string key)
            => _dataStore.ContainsKey(key) ? _dataStore[key] : default;

        public ValueTask<string> GetItemAsync(string key, CancellationToken cancellationToken = default)
            => new ValueTask<string>(GetItem(key));

        public string Key(int index)
            => index > _dataStore.Count - 1 ? default : _dataStore.ElementAt(index).Key;

        public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
            => new ValueTask<string>(Key(index));

        public IEnumerable<string> Keys()
        {
            return _dataStore.Keys.ToList();
        }

        public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
            => new ValueTask<IEnumerable<string>>(_dataStore.Keys.ToList());


        public int Length()
            => _dataStore.Count;

        public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
            => new ValueTask<int>(Length());

        public void RemoveItem(string key)
            => _dataStore.Remove(key);

        public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            RemoveItem(key);
            return new ValueTask(Task.CompletedTask);
        }

        public void RemoveItems(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                _dataStore.Remove(key);
            }
        }

        public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            RemoveItems(keys);

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

        public ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            SetItem(key, data);
            return new ValueTask(Task.CompletedTask);
        }
    }
}
