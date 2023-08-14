using System.Text.Json;

namespace Blazored.LocalStorage.StorageOptions
{
    public class LocalStorageOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions();
    }
}
