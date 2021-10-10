using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    internal interface IStorageProvider
    {
        void Clear();
        ValueTask ClearAsync(CancellationToken? cancellationToken = null);
        bool ContainKey(string key);
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null);
        string GetItem(string key);
        ValueTask<string> GetItemAsync(string key, CancellationToken? cancellationToken = null);
        string Key(int index);
        ValueTask<string> KeyAsync(int index, CancellationToken? cancellationToken = null);
        ValueTask<IEnumerable<string>> KeysAsync(CancellationToken? cancellationToken = null);
        int Length();
        ValueTask<int> LengthAsync(CancellationToken? cancellationToken = null);
        void RemoveItem(string key);
        ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null);
        void SetItem(string key, string data);
        ValueTask SetItemAsync(string key, string data, CancellationToken? cancellationToken = null);

    }
}
