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
    public class GetItem
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private const string Key = "testKey";

        public GetItem()
        {
            var mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimespanJsonConverter());
            mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            _serializer = new SystemTextJsonSerializer(mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(_storageProvider, _serializer);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void ThrowsArgumentNullException_When_KeyIsInvalid(string key)
        {
            // arrange / act
            var action = new Action(() => _sut.GetItem<object>(key));

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }
        
        [Theory]
        [InlineData("Item1", "stringTest")]
        [InlineData("Item2", 11)]
        [InlineData("Item3", 11.11)]
        public void ReturnsDeserializedDataFromStore<T>(string key, T data)
        {
            // Arrange
            _sut.SetItem(key, data);
            
            // Act
            var result = _sut.GetItem<T>(key);

            // Assert
            Assert.Equal(data, result);
        }

        [Fact]
        public void ReturnsComplexObjectFromStore()
        {
            // Arrange
            var objectToSave = new TestObject(2, "Jane Smith");
            _sut.SetItem(Key, objectToSave);

            // Act
            var result = _sut.GetItem<TestObject>(Key);

            // Assert
            Assert.Equal(objectToSave.Id, result.Id);
            Assert.Equal(objectToSave.Name, result.Name);
        }
        
        [Fact]
        public void ReturnsNullFromStore_When_NullValueSaved()
        {
            // Arrange
            var valueToSave = (string)null;
            _sut.SetItem(Key, valueToSave);

            // Act
            var result = _sut.GetItem<string>(Key);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public void ReturnsStringFromStore_When_JsonExceptionIsThrown()
        {
            // Arrange
            var jsonData = "[{ id: 5, name: \"Jane Smith\"}]";
            _storageProvider.SetItem(Key, jsonData);

            // Act
            var result = _sut.GetItem<string>(Key);

            // Assert
            Assert.Equal(jsonData, result);
        }
    }
}
