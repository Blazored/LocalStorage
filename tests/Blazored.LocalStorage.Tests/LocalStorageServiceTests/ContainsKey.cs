using System.Text.Json;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.TestExtensions;
using Blazored.LocalStorage.Tests.TestAssets;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class ContainsKey
    {
        private readonly LocalStorageService _sut;

        public ContainsKey()
        {
            var mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            IStorageProvider storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(storageProvider, serializer);
        }

        [Fact]
        public void ReturnsTrue_When_KeyExistsInStore()
        {
            // Arrange
            const string key = "TestKey";
            var item1 = new TestObject(1, "Jane Smith");
            _sut.SetItem(key, item1);
            
            // Act
            var containsKey = _sut.ContainKey(key);

            // Assert
            Assert.True(containsKey);
        }

        [Fact]
        public void ReturnsFalse_When_KeyDoesNotExistsInStore()
        {
            // Arrange
            const string key = "TestKey";
            
            // Act
            var containsKey = _sut.ContainKey(key);

            // Assert
            Assert.False(containsKey);
        }
    }
}
