using System;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage.Serialization;
using Microsoft.JSInterop;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService, ISyncLocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJsonSerializer _serializer;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public LocalStorageService(IJSRuntime jSRuntime, IJsonSerializer serializer)
        {
            _jSRuntime = jSRuntime;
            _serializer = serializer;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data).ConfigureAwait(false);

            if (e.Cancel)
                return;

            var serialisedData = _serializer.Serialize(data);
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, serialisedData).ConfigureAwait(false);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);

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
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public ValueTask RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public ValueTask ClearAsync() => _jSRuntime.InvokeVoidAsync("localStorage.clear");

        public ValueTask<int> LengthAsync() => _jSRuntime.InvokeAsync<int>("eval", "localStorage.length");

        public ValueTask<string> KeyAsync(int index) => _jSRuntime.InvokeAsync<string>("localStorage.key", index);

        public ValueTask<bool> ContainKeyAsync(string key) => _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);

        public void SetItem<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var e = RaiseOnChangingSync(key, data);

            if (e.Cancel)
                return;

            var serialisedData = _serializer.Serialize(data);
            _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, serialisedData);

            RaiseOnChanged(key, e.OldValue, data);
        }

        public T GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);

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
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
        }

        public void RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
        }

        public void Clear()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.InvokeVoid("localStorage.clear");
        }

        public int Length()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        }

        public string Key(int index)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<string>("localStorage.key", index);
        }

        public bool ContainKey(string key)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
        }

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

            var serialisedData = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);

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

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);

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
