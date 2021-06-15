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
    public class Length
    {
        private readonly LocalStorageService _sut;

        public Length()
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
        public void ReturnsZeroWhenStoreIsEmpty()
        {
            // Act
            var itemCount = _sut.Length();

            // Assert
            Assert.Equal(0, itemCount);
        }

        [Fact]
        public void ReturnsNumberOfItemsInStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem("Item1", item1);
            _sut.SetItem("Item2", item2);

            // Act
            var itemCount = _sut.Length();

            // Assert
            Assert.Equal(2, itemCount);
        }
    }
}
