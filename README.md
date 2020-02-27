# Blazored LocalStorage
A library to provide access to local storage in Blazor applications

[![Build Status](https://dev.azure.com/blazored/LocalStorage/_apis/build/status/Blazored.LocalStorage?branchName=master)](https://dev.azure.com/blazored/LocalStorage/_build/latest?definitionId=1&branchName=master)

![Nuget](https://img.shields.io/nuget/v/blazored.localstorage.svg)

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

With Blazor WebAssembly you also have the option of a synchronous API, if your use case requires it. You can swap the `ILocalStorageService` for `ISyncStorageService` which allows you to avoid use of `async`/`await`. For either interface, the method names are the same.

```c#
@inject Blazored.LocalStorage.ISyncStorageService localStorage

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

@functions {

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
  - RemoveItemAsync()
  - ClearAsync()
  - LengthAsync()
  - KeyAsync()
  - ContainsKeyAsync()
  
- synchronous via `ISyncStorageService` (Synchronous methods are **only** available in Blazor WebAssembly):
  - SetItem()
  - GetItem()
  - RemoveItem()
  - Clear()
  - Length()
  - Key()
  - ContainsKey()

**Note:** Blazored.LocalStorage methods will handle the serialisation and de-serialisation of the data for you.
