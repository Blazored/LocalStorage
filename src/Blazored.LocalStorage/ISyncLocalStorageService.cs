using System;

namespace Blazored.LocalStorage
{
    public interface ISyncLocalStorageService
    {
        void Clear();

        T GetItem<T>(string key);

        string Key(int index);

        /// <summary>
        /// Checks if the key exists in local storage but does not check the value.
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <returns>True if the key exist, false otherwise</returns>
        bool ContainKey(string key);

        int Length();

        void RemoveItem(string key);

        void SetItem<T>(string key, T data);

        event EventHandler<ChangingEventArgs> Changing;
        event EventHandler<ChangedEventArgs> Changed;
    }
}
