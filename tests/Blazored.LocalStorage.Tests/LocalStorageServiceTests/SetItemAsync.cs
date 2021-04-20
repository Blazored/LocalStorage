using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class SetItemAsync
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Mock<IOptions<LocalStorageOptions>> _mockOptions;
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private static readonly string _key = "testKey";

        public SetItemAsync()
        {
            _mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
            _mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            _serializer = new SystemTextJsonSerializer(_mockOptions.Object);
            _storageProvider = new InMemoryStorageProvider();
            _sut = new LocalStorageService(_storageProvider, _serializer);
        }

        [Fact]
        public async Task Should_OverwriteExistingValue()
        {
            // Arrange
            string existingValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNTg1NjYwNzEyLCJpc3MiOiJDb2RlUmVkQm9va2luZy5TZXJ2ZXIiLCJhdWQiOiJDb2RlUmVkQm9va2luZy5DbGllbnRzIn0.JhK1M1H7NLCFexujJYCDjTn9La0HloGYADMHXGCFksU";
            string newValue = "﻿6QLE0LL7iw7tHPAwold31qUENt3lVTUZxDGqeXQFx38=";

            var serializedValue = _serializer.Serialize(existingValue);
            await _storageProvider.SetItemAsync(_key, serializedValue);

            // Act
            await _sut.SetItemAsync(_key, newValue);

            // Assert
            var serializedUpdatedValue = await _storageProvider.GetItemAsync(_key);
            var updatedValue = _serializer.Deserialize<string>(serializedUpdatedValue);

            Assert.Equal(newValue, updatedValue);
        }
    }
}
