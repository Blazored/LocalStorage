using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage.Serialization;

namespace Blazored.LocalStorage
{
    internal class LocalStorageService : ILocalStorageService, ISyncLocalStorageService
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        public LocalStorageService(IStorageProvider storageProvider, IJsonSerializer serializer)
        {
            _storageProvider = storageProvider;
            _serializer = serializer;
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data).ConfigureAwait(false);

            if (e.Cancel)
                return;

            var serialisedData = _serializer.Serialize(data);
            await _storageProvider.SetItemAsync(key, serialisedData).ConfigureAwait(false);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _storageProvider.GetItemAsync(key).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return _serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string))
            {
                // For backward compatibility return the plain string.
                // On the next save a correct value will be stored and this Exception will not happen again, for this 'key'
                return (T)(object)serialisedData;
            }
        }

        public ValueTask<string> GetItemAsStringAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _storageProvider.GetItemAsync(key);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _storageProvider.RemoveItemAsync(key);
        }

        public ValueTask ClearAsync() 
            => _storageProvider.ClearAsync();

        public ValueTask<int> LengthAsync() 
            => _storageProvider.LengthAsync();

        public ValueTask<string> KeyAsync(int index) 
            => _storageProvider.KeyAsync(index);

        public ValueTask<bool> ContainKeyAsync(string key) 
            => _storageProvider.ContainKeyAsync(key);

        public void SetItem<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var e = RaiseOnChangingSync(key, data);

            if (e.Cancel)
                return;

            var serialisedData = _serializer.Serialize(data);
            _storageProvider.SetItem(key, serialisedData);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public T GetItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = _storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return _serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string))
            {
                // For backward compatibility return the plain string.
                // On the next save a correct value will be stored and this Exception will not happen again, for this 'key'
                return (T)(object)serialisedData;
            }
        }

        public string GetItemAsString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _storageProvider.GetItem(key);
        }

        public void RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _storageProvider.RemoveItem(key);
        }

        public void Clear()
            => _storageProvider.Clear();

        public int Length()
            => _storageProvider.Length();

        public string Key(int index)
            => _storageProvider.Key(index);

        public bool ContainKey(string key)
            => _storageProvider.ContainKey(key);

        public event EventHandler<ChangingEventArgs> Changing;
        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = await GetItemInternalAsync<object>(key).ConfigureAwait(false),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = GetItemInternal(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private async Task<T> GetItemInternalAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _storageProvider.GetItemAsync(key).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;
            try
            {
                return _serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException)
            {
                return (T)(object)serialisedData;
            }
        }

        private object GetItemInternal(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = _storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return _serializer.Deserialize<object>(serialisedData);
            }
            catch (JsonException)
            {
                return serialisedData;
            }
        }

        public event EventHandler<ChangedEventArgs> Changed;
        private void RaiseOnChanged(string key, object oldValue, object data)
        {
            var e = new ChangedEventArgs
            {
                Key = key,
                OldValue = oldValue,
                NewValue = data
            };

            Changed?.Invoke(this, e);
        }
    }
}
