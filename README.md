[![Nuget version](https://img.shields.io/nuget/v/blazored.localstorage.svg?logo=nuget)](https://www.nuget.org/packages/Blazored.LocalStorage/)
[![Nuget downloads](https://img.shields.io/nuget/dt/Blazored.LocalStorage?logo=nuget)](https://www.nuget.org/packages/Blazored.LocalStorage/)
![Build & Test Main](https://github.com/Blazored/LocalStorage/workflows/Build%20&%20Test%20Main/badge.svg)

# Blazored LocalStorage
Blazored LocalStorage is a library that provides access to the browsers local storage APIs for Blazor applications. An additional benefit of using this library is that it will handle serializing and deserializing values when saving or retrieving them.

## Breaking Changes (v3 > v4)

### JsonSerializerOptions
From v4 onwards we use the default the `JsonSerializerOptions` for `System.Text.Json` instead of using custom ones. This will cause values saved to local storage with v3 to break things.
To retain the old settings use the following configuration when adding Blazored LocalStorage to the DI container:

```csharp
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});
```

### SetItem[Async] method now serializes string values
Prior to v4 we bypassed the serialization of string values as it seemed a pointless as string can be stored directly. However, this led to some edge cases where nullable strings were being saved as the string `"null"`. Then when retrieved, instead of being null the value was `"null"`. By serializing strings this issue is taken care of. 
For those who wish to save raw string values, a new method `SetValueAsString[Async]` is available. This will save a string value without attempting to serialize it and will throw an exception if a null string is attempted to be saved.

## Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="Blazored.LocalStorage" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package Blazored.LocalStorage
```

If you're using Visual Studio you can also install via the built in NuGet package manager.

## Setup

You will need to register the local storage services with the service collection in your _Startup.cs_ file in Blazor Server.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredLocalStorage();
}
``` 

Or in your _Program.cs_ file in Blazor WebAssembly.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazoredLocalStorage();

    await builder.Build().RunAsync();
}
```

### Registering services as Singleton - Blazor WebAssembly **ONLY**
99% of developers will want to register Blazored LocalStorage using the method described above. However, in some very specific scenarios 
developer may have a need to register services as Singleton as apposed to Scoped. This is possible by using the following method:

```csharp
builder.Services.AddBlazoredLocalStorageAsSingleton();
```

This method will not work with Blazor Server applications as Blazor's JS interop services are registered as Scoped and cannot be injected into Singletons.

### Using JS Interop Streaming
When using interactive components in server-side apps JS Interop calls are limited to the configured SignalR message size (default: 32KB). 
Therefore when attempting to store or retrieve an object larger than this in LocalStorage the call will fail with a SignalR exception. 

The following streaming implementation can be used to remove this limit (you will still be limited by the browser).

Register the streaming local storage service 

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredLocalStorageStreaming();
}
``` 

Add the JavaScript file to your _App.razor_

```html
 <script src="_content/Blazored.LocalStorage/Blazored.LocalStorage.js"></script>
```

## Usage (Blazor WebAssembly)
To use Blazored.LocalStorage in Blazor WebAssembly, inject the `ILocalStorageService` per the example below.

```c#
@inject Blazored.LocalStorage.ILocalStorageService localStorage

@code {

    protected override async Task OnInitializedAsync()
    {
        await localStorage.SetItemAsync("name", "John Smith");
        var name = await localStorage.GetItemAsync<string>("name");
    }

}
```

With Blazor WebAssembly you also have the option of a synchronous API, if your use case requires it. You can swap the `ILocalStorageService` for `ISyncLocalStorageService` which allows you to avoid use of `async`/`await`. For either interface, the method names are the same.

```c#
@inject Blazored.LocalStorage.ISyncLocalStorageService localStorage

@code {

    protected override void OnInitialized()
    {
        localStorage.SetItem("name", "John Smith");
        var name = localStorage.GetItem<string>("name");
    }

}
```

## Usage (Blazor Server)

**NOTE:** Due to pre-rendering in Blazor Server you can't perform any JS interop until the `OnAfterRender` lifecycle method.

```c#
@inject Blazored.LocalStorage.ILocalStorageService localStorage

@code {

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await localStorage.SetItemAsync("name", "John Smith");
        var name = await localStorage.GetItemAsync<string>("name");
    }

}
```

The APIs available are:

- asynchronous via `ILocalStorageService`:
  - SetItemAsync()
  - SetItemAsStringAsync()
  - GetItemAsync()
  - GetItemAsStringAsync()
  - RemoveItemAsync()
  - ClearAsync()
  - LengthAsync()
  - KeyAsync()
  - ContainKeyAsync()
  
- synchronous via `ISyncLocalStorageService` (Synchronous methods are **only** available in Blazor WebAssembly):
  - SetItem()
  - SetItemAsString()
  - GetItem()
  - GetItemAsString()
  - RemoveItem()
  - Clear()
  - Length()
  - Key()
  - ContainKey()

**Note:** Blazored.LocalStorage methods will handle the serialisation and de-serialisation of the data for you, the exceptions are the `SetItemAsString[Async]` and `GetItemAsString[Async]` methods which will save and return raw string values from local storage.

## Configuring JSON Serializer Options
You can configure the options for the default serializer (System.Text.Json) when calling the `AddBlazoredLocalStorage` method to register services.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazoredLocalStorage(config =>
        config.JsonSerializerOptions.WriteIndented = true
    );

    await builder.Build().RunAsync();
}
```

## Using a custom JSON serializer
By default, the library uses `System.Text.Json`. If you prefer to use a different JSON library for serialization--or if you want to add some custom logic when serializing or deserializing--you can provide your own serializer which implements the `Blazored.LocalStorage.Serialization.IJsonSerializer` interface.

To register your own serializer in place of the default one, you can do the following:

```csharp
builder.Services.AddBlazoredLocalStorage();
builder.Services.Replace(ServiceDescriptor.Scoped<IJsonSerializer, MySerializer>());
```

You can find an example of this in the Blazor Server sample project. The standard serializer has been replaced with a new serializer which uses NewtonsoftJson.

## Testing with bUnit
The `Blazored.LocalStorage.TestExtensions` package provides test extensions for use with the [bUnit testing library](https://bunit.dev/). Using these test extensions will provide an in memory implementation which mimics local storage allowing more realistic testing of your components.

### Installing

To install the package add the following line to you csproj file replacing x.x.x with the latest version number (found at the top of this file):

```
<PackageReference Include="Blazored.LocalStorage.TestExtensions" Version="x.x.x" />
```

You can also install via the .NET CLI with the following command:

```
dotnet add package Blazored.LocalStorage.TestExtensions
```

If you're using Visual Studio you can also install via the built in NuGet package manager.

### Usage example

Below is an example test which uses these extensions. You can find an example project which shows this code in action in the samples folder.

```c#
public class IndexPageTests : TestContext
{
    [Fact]
    public async Task SavesNameToLocalStorage()
    {
        // Arrange
        const string inputName = "John Smith";
        var localStorage = this.AddBlazoredLocalStorage();
        var cut = RenderComponent<BlazorWebAssembly.Pages.Index>();

        // Act
        cut.Find("#Name").Change(inputName);
        cut.Find("#NameButton").Click();
            
        // Assert
        var name = await localStorage.GetItemAsync<string>("name");
            
        Assert.Equal(inputName, name);
    }
}
```
