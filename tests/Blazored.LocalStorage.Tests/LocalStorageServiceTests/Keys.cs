using System.Linq;
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
    public class Keys
    {
        private readonly LocalStorageService _sut;

        public Keys()
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
        public void ReturnsKeys()
        {
            // Arrange
            const string key1 = "TestKey1";
            const string key2 = "TestKey2";
            
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem(key1, item1);
            _sut.SetItem(key2, item2);

            // Act
            var keyNames = _sut.KeysAsync().Result.ToList();

            // Assert
            Assert.Collection(keyNames, 
                                item => Assert.Equal(key1, item),
                                item => Assert.Equal(key2, item));
        }

        [Fact]
        public void ReturnsEmptyWhenCollectionEmpty()
        {
            // Arrange
            _sut.Clear();

            // Act
            var keyNames = _sut.KeysAsync().Result;

            // Assert
            Assert.Empty(keyNames);
        }
    }
}
