using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        Task ClearAsync();

        Task<T> GetItemAsync<T>(string key);

        Task<string> KeyAsync(int index);

        Task<int> LengthAsync();

        Task RemoveItemAsync(string key);

        Task SetItemAsync(string key, object data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
