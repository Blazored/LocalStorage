using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public Task SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _jSRuntime.InvokeAsync<object>("Blazored.LocalStorage.SetItem", key, Json.Serialize(data));
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
    }
}
