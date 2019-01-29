using System.Threading.Tasks;

namespace Blazored.LocalStorage
{
    public interface ILocalStorageService
    {
        Task Clear();

        Task<T> GetItem<T>(string key);

        Task<string> Key(int index);

        Task<int> Length();

        Task RemoveItem(string key);

        Task SetItem(string key, object data);
    }
}
