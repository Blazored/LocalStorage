using System;
using System.Text.Json;
using System.Threading.Tasks;
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
    public class GetItemAsync
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private const string Key = "testKey";

        public GetItemAsync()
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
            var action = new Func<Task>(async () => await _sut.GetItemAsync<object>(key));

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(action);
        }
        
        [Theory]
        [InlineData("Item1", "stringTest")]
        [InlineData("Item2", 11)]
        [InlineData("Item3", 11.11)]
        public async Task ReturnsDeserializedDataFromStore<T>(string key, T data)
        {
            // Arrange
            await _sut.SetItemAsync(key, data);
            
            // Act
            var result = await _sut.GetItemAsync<T>(key);

            // Assert
            Assert.Equal(data, result);
        }

        [Fact]
        public async Task ReturnsComplexObjectFromStore()
        {
            // Arrange
            var objectToSave = new TestObject(2, "Jane Smith");
            await _sut.SetItemAsync(Key, objectToSave);

            // Act
            var result = await _sut.GetItemAsync<TestObject>(Key);

            // Assert
            Assert.Equal(objectToSave.Id, result.Id);
            Assert.Equal(objectToSave.Name, result.Name);
        }
        
        [Fact]
        public async Task ReturnsNullFromStore_When_NullValueSaved()
        {
            // Arrange
            var valueToSave = (string)null;
            await _sut.SetItemAsync(Key, valueToSave);

            // Act
            var result = await _sut.GetItemAsync<string>(Key);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task ReturnsStringFromStore_When_JsonExceptionIsThrown()
        {
            // Arrange
            var valueToSave = "[{ id: 5, name: \"Jane Smith\"}]";
            await _storageProvider.SetItemAsync(Key, valueToSave);

            // Act
            var result = await _sut.GetItemAsync<string>(Key);

            // Assert
            Assert.Equal(valueToSave, result);
        }
    }
}
