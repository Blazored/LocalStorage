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
    public class Clear
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;

        public Clear()
        {
            var mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(_storageProvider, serializer);
        }

        [Fact]
        public void ClearsAnyItemsInTheStore()
        {
            // Arrange
            var item1 = new TestObject(1, "Jane Smith");
            var item2 = new TestObject(2, "John Smith");
            
            _sut.SetItem("Item1", item1);
            _sut.SetItem("Item2", item2);

            // Act
            _sut.Clear();

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
        
        [Fact]
        public void DoesNothingWhenStoreIsEmpty()
        {
            // Act
            _sut.Clear();

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
    }
}
