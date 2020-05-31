using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService, ISyncLocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IJSInProcessRuntime _jSInProcessRuntime;
        private readonly JsonSerializerOptions _jsonOptions;

        public LocalStorageService(IJSRuntime jSRuntime, IOptions<LocalStorageOptions> options)
        {
            _jSRuntime = jSRuntime;
            _jsonOptions = options.Value.JsonSerializerOptions;
            _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public async Task SetItemAsync<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var e = await RaiseOnChangingAsync(key, data);

            if (e.Cancel)
                return;

            if (data is string)
            {
                await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, data);
            }
            else
            {
                var serialisedData = JsonSerializer.Serialize(data, _jsonOptions);
                await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, serialisedData);
            }

            RaiseOnChanged(key, e.OldValue, data);
        }

        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = (await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key))?.Trim();

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                || serialisedData.StartsWith("[") && serialisedData.EndsWith("]")
                || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\"")
                || typeof(T) != typeof(string))
            {
                return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
            }
            else
            {
                return (T)(object)serialisedData;
            }
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

            if (data is string)
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
            }
            else
            {
                _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, JsonSerializer.Serialize(data, _jsonOptions));
            }

            RaiseOnChanged(key, e.OldValue, data);
        }

        public T GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key)?.Trim();

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                || serialisedData.StartsWith("[") && serialisedData.EndsWith("]")
                || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\"")
                || typeof(T) != typeof(string))
            {
                return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
            }
            else
            {
                return (T)(object)serialisedData;
            }
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

        public IEnumerable<T> GetItems<T>()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var index = 0;

            for (var key = Key(index++); key != default; key = Key(index++))
            {
                var serialisedData = _jSInProcessRuntime.Invoke<string>("sessionStorage.getItem", key);

                if (serialisedData == default)
                {
                    continue;
                }

                if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                    || serialisedData.StartsWith("[") && serialisedData.EndsWith("]")
                    || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\"")
                    || typeof(T) != typeof(string))
                {
                    yield return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
                }
                else
                {
                    yield return (T)(object)serialisedData;
                }
            }
        }

        public IEnumerable<string> GetKeys()
        {
            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var index = 0;

            for (var key = Key(index++); key != default; key = Key(index++))
            {
                yield return key;
            }
        }

        public async IAsyncEnumerable<T> GetItemsAsync<T>([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var index = 0;

            for (var key = await KeyAsync(index++); key != default; key = await KeyAsync(index++))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                var serialisedData = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                if (serialisedData == default)
                {
                    continue;
                }

                if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                    || serialisedData.StartsWith("[") && serialisedData.EndsWith("]")
                    || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\"")
                    || typeof(T) != typeof(string))
                {
                    yield return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
                }
                else
                {
                    yield return (T)(object)serialisedData;
                }
            }
        }

        public async IAsyncEnumerable<string> GetKeysAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var index = 0;

            for (var key = await KeyAsync(index++); key != default; key = await KeyAsync(index++))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                yield return key;
            }
        }

        public event EventHandler<ChangingEventArgs> Changing;
        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = await GetItemInternalAsync<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        private async Task<T> GetItemInternalAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\""))
            {
                return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
            }
            else
            {
                return (T)(object)serialisedData;
            }
        }

        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                OldValue = GetItemInternal<object>(key),
                NewValue = data
            };

            Changing?.Invoke(this, e);

            return e;
        }

        public T GetItemInternal<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (_jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            if (serialisedData.StartsWith("{") && serialisedData.EndsWith("}")
                || serialisedData.StartsWith("\"") && serialisedData.EndsWith("\""))
            {
                return JsonSerializer.Deserialize<T>(serialisedData, _jsonOptions);
            }
            else
            {
                return (T)(object)serialisedData;
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
