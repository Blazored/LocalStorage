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
        private const string StorageNotAvailableMessage = "Unable to access the browser storage. This is most likely due to the browser settings.";
        
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;

        public BrowserStorageProvider(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask<string> GetItemAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _jSRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _jSRuntime.InvokeAsync<string>("localStorage.key", cancellationToken, index);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _jSRuntime.InvokeAsync<int>("eval", cancellationToken, "localStorage.length");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            try
            {
                await _jSRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _jSRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken, "Object.keys(localStorage)");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var key in keys)
                {
                    await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
                }
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void Clear()
        {
            CheckForInProcessRuntime();
            try
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.clear");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public string GetItem(string key)
        {
            CheckForInProcessRuntime();
            try
            {
                return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public string Key(int index)
        {
            CheckForInProcessRuntime();
            try
            {
                return _jSInProcessRuntime.Invoke<string>("localStorage.key", index);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public bool ContainKey(string key)
        {
            CheckForInProcessRuntime();
            try
            {
                return _jSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public int Length()
        {
            CheckForInProcessRuntime();
            try
            {
                return _jSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void RemoveItem(string key)
        {
            CheckForInProcessRuntime();
            try
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void RemoveItems(IEnumerable<string> keys)
        {
            CheckForInProcessRuntime();
            try
            {
                foreach (var key in keys)
                {
                    _jSInProcessRuntime.InvokeVoid("localStorage.removeItem", key);
                }
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void SetItem(string key, string data)
        {
            CheckForInProcessRuntime();
            try
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public IEnumerable<string> Keys()
        {
            CheckForInProcessRuntime();
            try
            {
                return _jSInProcessRuntime.Invoke<IEnumerable<string>>("eval", "Object.keys(localStorage)");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        private void CheckForInProcessRuntime()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");
        }

        private static bool IsStorageDisabledException(Exception exception) 
            => exception.Message.Contains("Failed to read the 'localStorage' property from 'Window'");
    }
}
