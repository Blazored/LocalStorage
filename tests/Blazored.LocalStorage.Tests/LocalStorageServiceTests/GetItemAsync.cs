using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.Tests.Mocks;
using Blazored.LocalStorage.Tests.TestAssets;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class GetItemAsync
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Mock<JSRuntimeWrapperAsync> _mockJSRuntime;
        private readonly Mock<IOptions<LocalStorageOptions>> _mockOptions;
        private readonly LocalStorageService _sut;

        private static readonly string _key = "testKey";

        public GetItemAsync()
        {
            _mockJSRuntime = new Mock<JSRuntimeWrapperAsync>();
            _mockOptions = new Mock<IOptions<LocalStorageOptions>>();
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
            _mockOptions.Setup(u => u.Value).Returns(new LocalStorageOptions());
            _sut = new LocalStorageService(_mockJSRuntime.Object, _mockOptions.Object);
        }

        [Theory]
        [InlineData("stringTest")]
        [InlineData(11)]
        [InlineData(11.11)]
        public async Task Should_DeserialiseToCorrectType<T>(T value)
        {
            // Arrange
            var serialisedData = "";
            if (typeof(T) == typeof(string))
                serialisedData = value.ToString();
            else
                serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.InvokeAsync<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => new ValueTask<string>(serialisedData));

            // Act
            var result = await _sut.GetItemAsync<T>(_key);

            // Assert
            Assert.Equal(value, result);
            _mockJSRuntime.Verify();
        }

        [Fact]
        public async Task Should_DeserialiseValueToNullableInt()
        {
            // Arrange
            int? value = 6;
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.InvokeAsync<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => new ValueTask<string>(serialisedData));

            // Act
            var result = await _sut.GetItemAsync<int?>(_key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task Should_DeserialiseValueToDecimal()
        {
            // Arrange
            decimal value = 6.00m;
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.InvokeAsync<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => new ValueTask<string>(serialisedData));

            // Act
            var result = await _sut.GetItemAsync<decimal>(_key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task Should_DeserialiseValueToComplexType()
        {
            // Arrange
            TestObject value = new TestObject { Id = 1, Name = "John Smith" };
            var serialisedData = JsonSerializer.Serialize(value, _jsonOptions);

            _mockJSRuntime.Setup(x => x.InvokeAsync<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => new ValueTask<string>(serialisedData));

            // Act
            var result = await _sut.GetItemAsync<TestObject>(_key);

            // Assert
            result.Should().BeEquivalentTo(value);
        }
    }
}
