using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        Task ClearAsync();

        Task<T> GetItemAsync<T>(string key);

        IAsyncEnumerable<string> GetKeysAsync(CancellationToken cancellationToken = default);

        Task<string> KeyAsync(int index);

        /// <summary>
        /// Checks if the key exists in local storage but does not check the value.
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        Task<bool> ContainKeyAsync(string key);

        Task<int> LengthAsync();

        Task RemoveItemAsync(string key);

        Task SetItemAsync<T>(string key, T data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
