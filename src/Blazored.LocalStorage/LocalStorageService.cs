using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService, ISyncLocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public Task SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            RaiseOnChanging(key, data, out ChangingEventArgs e);

            if (e.Cancel)
                return Task.CompletedTask;

            var jsResult = _jSRuntime.InvokeAsync<object>("Blazored.LocalStorage.SetItem", key, Json.Serialize(data));

            RaiseOnChanged(key, e.OldValue, data);

            return jsResult;
        }

        public async Task<T> GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jSRuntime.InvokeAsync<string>("Blazored.LocalStorage.GetItem", key);

            if (serialisedData == null)
                return default(T);

            return Json.Deserialize<T>(serialisedData);
        }

        public Task RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _jSRuntime.InvokeAsync<object>("Blazored.LocalStorage.RemoveItem", key);
        }

        public Task Clear() => _jSRuntime.InvokeAsync<object>("Blazored.LocalStorage.Clear");

        public Task<int> Length() => _jSRuntime.InvokeAsync<int>("Blazored.LocalStorage.Length");

        public Task<string> Key(int index) => _jSRuntime.InvokeAsync<string>("Blazored.LocalStorage.Key", index);

        void ISyncLocalStorageService.SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            RaiseOnChanging(key, data, out ChangingEventArgs e);

            if (e.Cancel)
                return;

            _jSInProcessRuntime.Invoke<object>("Blazored.LocalStorage.SetItem", key, Json.Serialize(data));

            RaiseOnChanged(key, e.OldValue, data);
        }

        T ISyncLocalStorageService.GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = _jSInProcessRuntime.Invoke<string>("Blazored.LocalStorage.GetItem", key);

            if (serialisedData == null)
                return default(T);

            return Json.Deserialize<T>(serialisedData);
        }

        void ISyncLocalStorageService.RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.Invoke<object>("Blazored.LocalStorage.RemoveItem", key);
        }

        void ISyncLocalStorageService.Clear()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            _jSInProcessRuntime.Invoke<object>("Blazored.LocalStorage.Clear");
        }

        int ISyncLocalStorageService.Length()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<int>("Blazored.LocalStorage.Length");
        }

        string ISyncLocalStorageService.Key(int index)
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            return _jSInProcessRuntime.Invoke<string>("Blazored.LocalStorage.Key", index);
        }

        private object GetItem(string key)
            => ((ISyncLocalStorageService)this).GetItem<object>(key);

        public event EventHandler<ChangingEventArgs> Changing;
        private void RaiseOnChanging(string key, object data, out ChangingEventArgs e)
        {
            e = new ChangingEventArgs
            {
                Key = key,
                OldValue = GetItem(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);
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
