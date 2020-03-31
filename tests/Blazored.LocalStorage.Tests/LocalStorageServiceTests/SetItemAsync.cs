using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Tests.Mocks;
using FluentAssertions;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests
{
    public class SetItemAsync
    {
        private JsonSerializerOptions _jsonOptions;
        private Mock<JSRuntimeWrapperAsync> _mockJSRuntime;
        private LocalStorageService _sut;

        private static string _key = "testKey";

        public SetItemAsync()
        {
            _mockJSRuntime = new Mock<JSRuntimeWrapperAsync>();
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new TimespanJsonConverter());
            _sut = new LocalStorageService(_mockJSRuntime.Object);
        }

        [Fact]
        public async Task Should_OverwriteExistingValue()
        {
            // Arrange
            string existingValue = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiZXhwIjoxNTg1NjYwNzEyLCJpc3MiOiJDb2RlUmVkQm9va2luZy5TZXJ2ZXIiLCJhdWQiOiJDb2RlUmVkQm9va2luZy5DbGllbnRzIn0.JhK1M1H7NLCFexujJYCDjTn9La0HloGYADMHXGCFksU";
            string newValue = "﻿6QLE0LL7iw7tHPAwold31qUENt3lVTUZxDGqeXQFx38=";

            _mockJSRuntime.Setup(x => x.InvokeAsync<string>("localStorage.getItem", new[] { _key }))
                          .Returns(() => new ValueTask<string>(existingValue));
            _mockJSRuntime.Setup(x => x.InvokeVoidAsync("localStorage.setItem", new[] { _key, newValue }));

            // Act
            await _sut.SetItemAsync(_key, newValue);

            // Assert
            _mockJSRuntime.Verify();
        }
    }
}
