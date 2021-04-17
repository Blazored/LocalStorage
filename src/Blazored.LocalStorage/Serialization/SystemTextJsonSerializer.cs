using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Blazored.LocalStorage.Serialization
{
    public sealed class SystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<LocalStorageOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T Deserialize<T>(string data)
        {
            return JsonSerializer.Deserialize<T>(data, _options);
        }

        public string Serialize<T>(T data)
        {
            return JsonSerializer.Serialize(data, _options);
        }
    }
}
