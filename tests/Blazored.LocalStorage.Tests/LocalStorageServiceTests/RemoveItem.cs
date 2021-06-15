using System;
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
    public class RemoveItem
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;

        private const string Key = "testKey";

        public RemoveItem()
        {
            var mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            IJsonSerializer serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(_storageProvider, serializer);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ThrowsArgumentNullException_When_KeyIsInvalid(string key)
        {
            // arrange / act
            var action = new Action(() => _sut.RemoveItem(key));

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void RemovesItemFromStoreIfExists()
        {
            // Arrange
            var data = new TestObject(2, "Jane Smith");
            _sut.SetItem(Key, data);

            // Act
            _sut.RemoveItem(Key);

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
        
        [Fact]
        public void DoesNothingWhenItemDoesNotExistInStore()
        {
            // Act
            _sut.RemoveItem(Key);

            // Assert
            Assert.Equal(0, _storageProvider.Length());
        }
    }
}
