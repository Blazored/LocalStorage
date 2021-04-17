# Blazored LocalStorage
A library to provide access to local storage in Blazor applications

![Build & Test Main](https://github.com/Blazored/LocalStorage/workflows/Build%20&%20Test%20Main/badge.svg)

[![Nuget](https://img.shields.io/nuget/v/blazored.localstorage.svg)](https://www.nuget.org/packages/Blazored.LocalStorage/)

## v3 > v4 Breaking Changes with JsonSerializerOptions
From v4 onwards we use the default the `JsonSerializerOptions` for `System.Text.Json` instead of using custom ones. This will cause values saved to local storage with v3 to break things.
To retain the old settings use the following configuration when adding Blazored LocalStorage to the DI container:

```csharp
builder.Services.AddBlazoredLocalStorage(config =>
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.IgnoreNullValues = true;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
);
```

### Installing

You can install from NuGet using the following command:

`Install-Package Blazored.LocalStorage`

Or via the Visual Studio package manager.

### Setup

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

### Configuration

The local storage provides options that can be modified by you at registration in your _Startup.cs_ file in Blazor Server.


```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredLocalStorage(config =>
        config.JsonSerializerOptions.WriteIndented = true);
}
```
Or in your _Program.cs_ file in Blazor WebAssembly.

```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("app");

    builder.Services.AddBlazoredLocalStorage(config =>
        config.JsonSerializerOptions.WriteIndented = true);

    await builder.Build().RunAsync();
}
```

### Usage (Blazor WebAssembly)
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

### Usage (Blazor Server)

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
  - GetItemAsync()
  - GetItemAsStringAsync()
  - RemoveItemAsync()
  - ClearAsync()
  - LengthAsync()
  - KeyAsync()
  - ContainKeyAsync()
  
- synchronous via `ISyncLocalStorageService` (Synchronous methods are **only** available in Blazor WebAssembly):
  - SetItem()
  - GetItem()
  - GetItemAsString()
  - RemoveItem()
  - Clear()
  - Length()
  - Key()
  - ContainKey()

**Note:** Blazored.LocalStorage methods will handle the serialisation and de-serialisation of the data for you, the exception is the `GetItemAsString[Async]` method.

If you want to handle serialising and de-serialising yourself, serialise the data to a string and save using the `SetItem[Async]` method, as normal -- This method will not attempt to serialise a string value. You can then read out the data using the `GetItemAsString[Async]` method and de-serialise it yourself.
