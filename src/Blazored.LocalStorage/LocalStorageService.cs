using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public class LocalStorageService : ILocalStorageService
    {
        public Task SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return JSRuntime.Current.InvokeAsync<object>("Blazored.LocalStorage.SetItem", key, Json.Serialize(data));
        }

        public async Task<T> GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await JSRuntime.Current.InvokeAsync<string>("Blazored.LocalStorage.GetItem", key);

            if (serialisedData == null)
                return default(T);

            return Json.Deserialize<T>(serialisedData);
        }

        public Task RemoveItem(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return JSRuntime.Current.InvokeAsync<object>("Blazored.LocalStorage.RemoveItem", key);
        }

        public Task Clear() => JSRuntime.Current.InvokeAsync<object>("Blazored.LocalStorage.Clear");

        public Task<int> Length() => JSRuntime.Current.InvokeAsync<int>("Blazored.LocalStorage.Length");

        public Task<string> Key(int index) => JSRuntime.Current.InvokeAsync<string>("Blazored.LocalStorage.Key", index);
    }
}
