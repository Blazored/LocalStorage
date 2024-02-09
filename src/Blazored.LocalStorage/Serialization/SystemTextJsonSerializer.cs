using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Blazored.LocalStorage.Serialization
{
    internal class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<LocalStorageOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public SystemTextJsonSerializer(LocalStorageOptions localStorageOptions)
        {
            _options = localStorageOptions.JsonSerializerOptions;
        }

        public T? Deserialize<T>(string data) 
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}
