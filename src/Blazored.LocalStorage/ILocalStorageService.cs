using System;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        Task ClearAsync();

        Task<T> GetItemAsync<T>(string key);

        Task<string> KeyAsync(int index);

        /// <summary>
        /// Check if the key exist in the local storage, don't check the value
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        Task<bool> ContainKeyAsync(string key);

        Task<int> LengthAsync();

        Task RemoveItemAsync(string key);

        Task SetItemAsync(string key, object data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
