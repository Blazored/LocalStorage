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
    public class SetItemAsync
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private const string Key = "testKey";

        public SetItemAsync()
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
            const string data = "Data";
            var action = new Func<Task>(async () => await _sut.SetItemAsync(key, data));

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(action);
        }

        [Fact]
        public async Task RaisesOnChangingEvent_When_SavingNewData()
        {
            // arrange
            var onChangingCalled = false;
            _sut.Changing += (sender, args) => onChangingCalled = true;

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.True(onChangingCalled);
        }
        
        [Fact]
        public async Task OnChangingEventContainsEmptyOldValue_When_SavingData()
        {
            // arrange
            var oldValue = "";
            _sut.Changing += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.Equal(default, oldValue);
        }
        
        [Fact]
        public async Task OnChangingEventContainsNewValue_When_SavingNewData()
        {
            // arrange
            const string data = "Data";
            var newValue = "";
            _sut.Changing += (_, args) => newValue = args.NewValue.ToString();

            // act
            await _sut.SetItemAsync("Key", data);

            // assert
            Assert.Equal(data, newValue);
        }
        
        [Fact]
        public async Task OnChangingEventIsCancelled_When_SettingCancelToTrue_When_SavingNewData()
        {
            // arrange
            _sut.Changing += (_, args) => args.Cancel = true;

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.Equal(0, await _storageProvider.LengthAsync());
        }
        
        [Theory]
        [InlineData("stringTest")]
        [InlineData(11)]
        [InlineData(11.11)]
        public async Task SavesDataAsJsonToStore<T>(T valueToSave)
        {
            // Act
            await _sut.SetItemAsync(Key, valueToSave);

            // Assert
            var serializedValue = await _storageProvider.GetItemAsync(Key);
            var valueFromStore = _serializer.Deserialize<T>(serializedValue);

            Assert.Equal(1, await _storageProvider.LengthAsync());
            Assert.Equal(valueToSave, valueFromStore);
        }

        [Fact]
        public async Task SavesComplexObjectAsJsonToStore()
        {
            // Arrange
            var objectToSave = new TestObject(2, "Jane Smith");

            // Act
            await _sut.SetItemAsync(Key, objectToSave);

            // Assert
            var serializedObject = await _storageProvider.GetItemAsync(Key);
            var objectFromStore = _serializer.Deserialize<TestObject>(serializedObject);

            Assert.Equal(1, await _storageProvider.LengthAsync());
            Assert.Equal(objectToSave.Id, objectFromStore.Id);
            Assert.Equal(objectToSave.Name, objectFromStore.Name);
        }
        
        [Fact]
        public async Task SavesNullIntoStore_When_NullValueProvided()
        {
            // Arrange
            var valueToSave = (string)null;

            // Act
            await _sut.SetItemAsync(Key, valueToSave);

            // Assert
            var serializedValue = await _storageProvider.GetItemAsync(Key);
            var valueFromStore = _serializer.Deserialize<string>(serializedValue);

            Assert.Equal(1, await _storageProvider.LengthAsync());
            Assert.Null(valueFromStore);
        }
        
        [Fact]
        public async Task OverwriteExistingValueInStore_When_UsingTheSameKey()
        {
            // Arrange
            const string existingValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNTg1NjYwNzEyLCJpc3MiOiJDb2RlUmVkQm9va2luZy5TZXJ2ZXIiLCJhdWQiOiJDb2RlUmVkQm9va2luZy5DbGllbnRzIn0.JhK1M1H7NLCFexujJYCDjTn9La0HloGYADMHXGCFksU";
            const string newValue = "﻿6QLE0LL7iw7tHPAwold31qUENt3lVTUZxDGqeXQFx38=";

            var serializedValue = _serializer.Serialize(existingValue);
            await _storageProvider.SetItemAsync(Key, serializedValue);

            // Act
            await _sut.SetItemAsync(Key, newValue);

            // Assert
            var serializedUpdatedValue = await _storageProvider.GetItemAsync(Key);
            var updatedValue = _serializer.Deserialize<string>(serializedUpdatedValue);

            Assert.Equal(newValue, updatedValue);
        }
        
        [Fact]
        public async Task RaisesOnChangedEvent_When_SavingData()
        {
            // arrange
            var onChangedCalled = false;
            _sut.Changed += (sender, args) => onChangedCalled = true;

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.True(onChangedCalled);
        }
        
        [Fact]
        public async Task OnChangedEventContainsEmptyOldValue_When_SavingNewData()
        {
            // arrange
            var oldValue = "";
            _sut.Changed += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.Equal(default, oldValue);
        }
        
        [Fact]
        public async Task OnChangedEventContainsNewValue_When_SavingNewData()
        {
            // arrange
            const string data = "Data";
            var newValue = "";
            _sut.Changed += (_, args) => newValue = args.NewValue.ToString();

            // act
            await _sut.SetItemAsync("Key", data);

            // assert
            Assert.Equal(data, newValue);
        }
        
        [Fact]
        public async Task OnChangedEventContainsOldValue_When_UpdatingExistingData()
        {
            // arrange
            var existingValue = "Foo";
            await _storageProvider.SetItemAsync("Key", existingValue);
            var oldValue = "";
            _sut.Changed += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            await _sut.SetItemAsync("Key", "Data");

            // assert
            Assert.Equal(existingValue, oldValue);
        }
    }
}
