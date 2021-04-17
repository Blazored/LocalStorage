using System.Text.Json;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.Tests.Mocks;
using Blazored.LocalStorage.Tests.TestAssets;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class GetItem
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Mock<JSRuntimeWrapper> _mockJSRuntime;
        private readonly Mock<IOptions<LocalStorageOptions>> _mockOptions;
        private readonly LocalStorageService _sut;

        private static readonly string _key = "testKey";

        public GetItem()
        {
            _mockJSRuntime = new Mock<JSRuntimeWrapper>();
            _mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
            _mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            var serializer = new SystemTextJsonSerializer(_mockOptions.Object);
            _sut = new LocalStorageService(_mockJSRuntime.Object, serializer);
        }

        [Theory]
        [InlineData("stringTest")]
        [InlineData(11)]
        [InlineData(11.11)]
        public void Should_DeserialiseToCorrectType<T>(T value)
        {
            // Arrange
            var serialisedData = "";
            if (typeof(T) == typeof(string))
                serialisedData = value.ToString();
            else
                serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.Invoke<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => serialisedData);

            // Act
            var result = _sut.GetItem<T>(_key);

            // Assert
            Assert.Equal(value, result);
            _mockJSRuntime.Verify();
        }

        [Fact]
        public void Should_DeserialiseValueToNullableInt()
        {
            // Arrange
            int? value = 6;
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.Invoke<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => serialisedData);

            // Act
            var result = _sut.GetItem<int?>(_key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Should_DeserialiseValueToDecimal()
        {
            // Arrange
            decimal value = 6.00m;
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.Invoke<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => serialisedData);

            // Act
            var result = _sut.GetItem<decimal>(_key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Should_DeserialiseValueToComplexType()
        {
            // Arrange
            TestObject value = new TestObject { Id = 1, Name = "John Smith" };
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.Invoke<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => serialisedData);

            // Act
            var result = _sut.GetItem<TestObject>(_key);

            // Assert
            result.Should().BeEquivalentTo(value);
        }
    }
}
