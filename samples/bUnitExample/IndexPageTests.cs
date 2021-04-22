using Bunit;
using System.Text.Json;
using Xunit;

namespace bUnitExample
{
    public class IndexPageTests : TestContext
    {
        [Fact]
        public void SavesNameToLocalStorage()
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
