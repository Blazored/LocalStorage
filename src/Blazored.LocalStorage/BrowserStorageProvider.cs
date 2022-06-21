using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

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

        public ValueTask ClearAsync(CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);

        public ValueTask<string> GetItemAsync(string key, CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);

        public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeAsync<string>("localStorage.key", cancellationToken, index);

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key);

        public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeAsync<int>("eval", cancellationToken, "localStorage.length");

        public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);

        public ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);

        public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
            => _jSRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken, "Object.keys(localStorage)");

        public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
                }
            }

            return new ValueTask(Task.CompletedTask);
        }

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
            _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
        }

        public void RemoveItems(IEnumerable<string> keys)
        {
            CheckForInProcessRuntime();
            foreach (var key in keys)
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
            }
        }

        public void SetItem(string key, string data)
        {
            CheckForInProcessRuntime();
            _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
        }

        public IEnumerable<string> Keys()
        {
            CheckForInProcessRuntime();
            return _jSInProcessRuntime.Invoke<IEnumerable<string>>("eval", "Object.keys(localStorage)");
        }

        private void CheckForInProcessRuntime()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");
        }
    }
}
