using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    internal interface IStorageProvider
    {
        void Clear();
        ValueTask ClearAsync(CancellationToken cancellationToken = default);
        bool ContainKey(string key);
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);
        string? GetItem(string key);
        ValueTask<string?> GetItemAsync(string key, CancellationToken cancellationToken = default);
        string? Key(int index);
        ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default);
        IEnumerable<string> Keys();
        ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default);
        int Length();
        ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);
        void RemoveItem(string key);
        ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);
        void RemoveItems(IEnumerable<string> keys);
        ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);
        void SetItem(string key, string data);
        ValueTask SetItemAsync(string key, string data, CancellationToken cancellationToken = default);

    }
}
