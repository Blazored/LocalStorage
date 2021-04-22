using Bunit;
using Bunit.TestDoubles;
using System;
using System.Text.Json;
using Blazored.LocalStorage.Testing;
using Xunit;

namespace bUnitExample
{
    /// <summary>
    /// These tests are written entirely in C#.
    /// Learn more at https://bunit.egilhansen.com/docs/getting-started/
    /// </summary>
    public class IndexPageTests : TestContext
    {
        [Fact]
        public void CounterStartsAtZero()
        {
            // Arrange
            const string inputName = "John Smith";
            var storageProvider = this.AddBlazoredLocalStorage();
            var cut = RenderComponent<BlazorWebAssembly.Pages.Index>();

            // Act
            cut.Find("#Name").Change(inputName);
            cut.Find("#NameButton").Click();
            
            // Assert
            var serializedName = storageProvider.GetItem("name");
            var name = JsonSerializer.Deserialize<string>(serializedName);
            
            Assert.Equal(inputName, name);
        }
    }
}
