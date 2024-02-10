using System;
using System.Collections.Generic;

namespace Blazored.LocalStorage
{
    public interface ISyncLocalStorageService
    {
        /// <summary>
        /// Clears all data from local storage.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieve the specified data from local storage as a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the local storage slot to use</param>
        /// <returns>The data from the specified <paramref name="key"/> as a <typeparamref name="T"/></returns>
        T? GetItem<T>(string key);

        /// <summary>
        /// Retrieve the specified data from local storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>The data associated with the specified <paramref name="key"/> as a <see cref="string"/></returns>
        string? GetItemAsString(string key);

        /// <summary>
        /// Return the name of the key at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The name of the key at the specified <paramref name="index"/></returns>
        string? Key(int index);

        /// <summary>
        /// Checks if the <paramref name="key"/> exists in local storage, but does not check its value.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <returns>Boolean indicating if the specified <paramref name="key"/> exists</returns>
        bool ContainKey(string key);

        /// <summary>
        /// The number of items stored in local storage.
        /// </summary>
        /// <returns>The number of items stored in local storage</returns>
        int Length();

        /// <summary>
        /// Get the keys of all items stored in local storage.
        /// </summary>
        /// <returns>The keys of all items stored in local storage</returns>
        IEnumerable<string> Keys();

        /// <summary>
        /// Remove the data with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to remove</param>
        void RemoveItem(string key);

        /// <summary>
        /// Removes a collection of <paramref name="keys"/>.
        /// </summary>
        /// <param name="keys">A IEnumerable collection of strings specifying the name of the storage slot to remove</param>
        void RemoveItems(IEnumerable<string> keys);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The data to be saved</param>
        void SetItem<T>(string key, T data);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <returns></returns>
        void SetItemAsString(string key, string data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
