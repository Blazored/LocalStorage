using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage.Exceptions;

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

        public ValueTask ClearAsync(CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken ?? CancellationToken.None);

        public ValueTask<string> GetItemAsync(string key, CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken ?? CancellationToken.None, key);

        public ValueTask<string> KeyAsync(int index, CancellationToken? cancellationToken = null)
        {
            try
            {
                return _jSRuntime.InvokeAsync<string>("localStorage.key", cancellationToken ?? CancellationToken.None, index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken ?? CancellationToken.None, key);

        public ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeAsync<int>("eval", cancellationToken ?? CancellationToken.None, "localStorage.length");

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken ?? CancellationToken.None, key);

        public ValueTask SetItemAsync(string key, string data, CancellationToken? cancellationToken = null)
            => _jSRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken ?? CancellationToken.None, key, data);

        public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken? cancellationToken = null)
        {
            try
            {
                
                return await _jSRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken ?? CancellationToken.None, "Object.keys(localStorage)");
            }
            catch (Exception e)
            {
                throw new BrowserStorageDisabledException("Unable to access the browser storage. This is most likely due to the browser settings.", e);
            }
        }

        public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken? cancellationToken = null)
        {
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken ?? CancellationToken.None, key);
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
