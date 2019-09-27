using Blazored.LocalStorage.JsonConverters;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService, ISyncLocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        private JsonSerializerOptions _jsonOptions;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
        }

        public async Task SetItemAsync(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data);

            if (e.Cancel)
                return;

            await _jSRuntime.InvokeAsync<object>("localStorage.setItem", key, JsonSerializer.Serialize(data, _jsonOptions));

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default(T);

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            await _jSRuntime.InvokeAsync<object>("localStorage.removeItem", key);
        }

        public async Task ClearAsync() => await _jSRuntime.InvokeAsync<object>("localStorage.clear");

        public async Task<int> LengthAsync() => await _jSRuntime.InvokeAsync<int>("eval", "localStorage.length");

        public async Task<string> KeyAsync(int index) => await _jSRuntime.InvokeAsync<string>("localStorage.key", index);

        public async Task<bool> ContainKeyAsync(string key) => await _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);

        public void SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var e = RaiseOnChangingSync(key, data);

            if (e.Cancel)
                return;

            _jSInProcessRuntime.Invoke<object>("localStorage.setItem", key, JsonSerializer.Serialize(data, _jsonOptions));

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
                return default(T);

            return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
        }

        public void RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
                
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.Invoke<object>("localStorage.removeItem", key);
        }

        public void Clear()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.Invoke<object>("localStorage.clear");
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
                OldValue = await GetItemAsync<object>(key),
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
                OldValue = ((ISyncLocalStorageService)this).GetItem<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
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
