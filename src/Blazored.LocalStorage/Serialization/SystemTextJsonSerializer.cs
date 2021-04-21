using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("Blazored.LocalStorage.Tests, PublicKey=" +
                              "0024000004800000940000000602000000240000525341310004000001000100e94102d6760ebc"+
"ff1970798791888ddf102ac709e19db9a312721fafca42b894652b59bada7d592a4ab62a5b7650" +
"7a27720e922bc310c4f5aa75acd8ab59632c920ac41a7e9abcaf4b8bb5525a60931faccea704db" +
    "dcf68e1207616751447dcfec687f18854148aa66a9a09e1edc8fd0c9bd11950b4baf7d46fe38993af4add4")]
namespace Blazored.LocalStorage.Serialization
{
    internal class SystemTextJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public SystemTextJsonSerializer(IOptions<LocalStorageOptions> options)
        {
            _options = options.Value.JsonSerializerOptions;
        }

        public T Deserialize<T>(string data) 
            => JsonSerializer.Deserialize<T>(data, _options);

        public string Serialize<T>(T data)
            => JsonSerializer.Serialize(data, _options);
    }
}
