using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    internal class BrowserStorageProvider : IStorageProvider
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public BrowserStorageProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public ValueTask ClearAsync()
            => _jSRuntime.InvokeVoidAsync("localStorage.clear");

        public ValueTask<string> GetItemAsync(string key)
            => _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);

        public ValueTask<string> KeyAsync(int index)
            => _jSRuntime.InvokeAsync<string>("localStorage.key", index);

        public ValueTask<bool> ContainKeyAsync(string key)
            => _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);

        public ValueTask<int> LengthAsync()
            => _jSRuntime.InvokeAsync<int>("eval", "localStorage.length");

        public ValueTask RemoveItemAsync(string key)
            => _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

        public ValueTask SetItemAsync(string key, string data)
            => _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, data);

        public void Clear()
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("localStorage.clear");
        }

        public string GetItem(string key)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
        }

        public string Key(int index)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<string>("localStorage.key", index);
        }

        public bool ContainKey(string key)
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
        }

        public int Length()
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        }

        public void RemoveItem(string key)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public void SetItem(string key, string data)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
        }

        private void CheckForInProcessRuntime()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");
        }
    }
}
