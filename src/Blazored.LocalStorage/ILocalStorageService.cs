using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        ValueTask ClearAsync();

        ValueTask<T> GetItemAsync<T>(string key);

        IAsyncEnumerable<T> GetItemsAsync<T>(CancellationToken cancellationToken = default);

        IAsyncEnumerable<string> GetKeysAsync(CancellationToken cancellationToken = default);

        ValueTask<string> KeyAsync(int index);

        /// <summary>
        /// Checks if the key exists in local storage but does not check the value.
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        ValueTask<bool> ContainKeyAsync(string key);

        ValueTask<int> LengthAsync();

        ValueTask RemoveItemAsync(string key);

        Task SetItemAsync<T>(string key, T data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
