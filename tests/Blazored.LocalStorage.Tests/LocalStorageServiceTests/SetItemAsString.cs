using System;
using System.Text.Json;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.TestExtensions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class SetItemAsString
    {
        private readonly LocalStorageService _sut;
        private readonly IStorageProvider _storageProvider;
        private readonly IJsonSerializer _serializer;

        private const string Key = "testKey";

        public SetItemAsString()
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
            var action = new Action(() => _sut.SetItemAsString(key, data));

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }
        
        [Fact]
        public void ThrowsArgumentNullException_When_DataIsNull()
        {
            // arrange / act
            var data = (string)null;
            var action = new Action(() => _sut.SetItemAsString("MyValue", data));

            // assert
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void RaisesOnChangingEvent_When_SavingNewData()
        {
            // arrange
            var onChangingCalled = false;
            _sut.Changing += (_, _) => onChangingCalled = true;

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.True(onChangingCalled);
        }
        
        [Fact]
        public void OnChangingEventContainsEmptyOldValue_When_SavingData()
        {
            // arrange
            var oldValue = "";
            _sut.Changing += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.Equal(default, oldValue);
        }
        
        [Fact]
        public void OnChangingEventContainsNewValue_When_SavingNewData()
        {
            // arrange
            const string data = "Data";
            var newValue = "";
            _sut.Changing += (_, args) => newValue = args.NewValue.ToString();

            // act
            _sut.SetItemAsString("Key", data);

            // assert
            Assert.Equal(data, newValue);
        }
        
        [Fact]
        public void OnChangingEventIsCancelled_When_SettingCancelToTrue_When_SavingNewData()
        {
            // arrange
            _sut.Changing += (_, args) => args.Cancel = true;

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.Equal(0, _storageProvider.Length());
        }
        
        [Fact]
        public void SavesDataToStore()
        {
            // Act
            var valueToSave = "StringValue";
            _sut.SetItemAsString(Key, valueToSave);

            // Assert
            var valueFromStore = _storageProvider.GetItem(Key);

            Assert.Equal(1, _storageProvider.Length());
            Assert.Equal(valueToSave, valueFromStore);
        }
        
        [Fact]
        public void OverwriteExistingValueInStore_When_UsingTheSameKey()
        {
            // Arrange
            const string existingValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNTg1NjYwNzEyLCJpc3MiOiJDb2RlUmVkQm9va2luZy5TZXJ2ZXIiLCJhdWQiOiJDb2RlUmVkQm9va2luZy5DbGllbnRzIn0.JhK1M1H7NLCFexujJYCDjTn9La0HloGYADMHXGCFksU";
            const string newValue = "﻿6QLE0LL7iw7tHPAwold31qUENt3lVTUZxDGqeXQFx38=";

            _storageProvider.SetItem(Key, existingValue);

            // Act
            _sut.SetItemAsString(Key, newValue);

            // Assert
            var updatedValue = _storageProvider.GetItem(Key);

            Assert.Equal(newValue, updatedValue);
        }
        
        [Fact]
        public void RaisesOnChangedEvent_When_SavingData()
        {
            // arrange
            var onChangedCalled = false;
            _sut.Changed += (_, _) => onChangedCalled = true;

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.True(onChangedCalled);
        }
        
        [Fact]
        public void OnChangedEventContainsEmptyOldValue_When_SavingNewData()
        {
            // arrange
            var oldValue = "";
            _sut.Changed += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.Equal(default, oldValue);
        }
        
        [Fact]
        public void OnChangedEventContainsNewValue_When_SavingNewData()
        {
            // arrange
            const string data = "Data";
            var newValue = "";
            _sut.Changed += (_, args) => newValue = args.NewValue.ToString();

            // act
            _sut.SetItemAsString("Key", data);

            // assert
            Assert.Equal(data, newValue);
        }
        
        [Fact]
        public void OnChangedEventContainsOldValue_When_UpdatingExistingData()
        {
            // arrange
            var existingValue = "Foo";
            _storageProvider.SetItem("Key", existingValue);
            var oldValue = "";
            _sut.Changed += (_, args) => oldValue = args.OldValue?.ToString();

            // act
            _sut.SetItemAsString("Key", "Data");

            // assert
            Assert.Equal(existingValue, oldValue);
        }
    }
}
