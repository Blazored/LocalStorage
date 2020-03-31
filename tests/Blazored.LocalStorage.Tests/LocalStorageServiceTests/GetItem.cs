using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Tests.Mocks;
using Blazored.LocalStorage.Tests.TestAssets;
using FluentAssertions;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class GetItem
    {
        private JsonSerializerOptions _jsonOptions;
        private Mock<JSRuntimeWrapper> _mockJSRuntime;
        private LocalStorageService _sut;

        private static string _key = "testKey";

        public GetItem()
        {
            _mockJSRuntime = new Mock<JSRuntimeWrapper>();
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
            _sut = new LocalStorageService(_mockJSRuntime.Object);
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
