# Blazored LocalStorage
A library to provide access to local storage in Blazor applications

[![Build Status](https://dev.azure.com/blazored/LocalStorage/_apis/build/status/Blazored.LocalStorage?branchName=master)](https://dev.azure.com/blazored/LocalStorage/_build/latest?definitionId=1&branchName=master)

![Nuget](https://img.shields.io/nuget/v/blazored.localstorage.svg)

### Installing

You can install from NuGet using the following command:

`Install-Package Blazored.LocalStorage`

Or via the Visual Studio package manger.

### Setup

If you are using server-side Blazor you will need to add a reference to the Blazored LocalStorage javascript file in your `_Host.cshtml` file.

```
<script src="_content/Blazored.LocalStorage/blazored-localstorage.js"></script>
```

If you are using client-side Blazor this reference will be added automatically for you.

You will then need to register the local storage services with the service collection in your _startup.cs_ file.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredLocalStorage();
}
``` 

### Usage (Client-side Blazor)
To use Blazored.LocalStorage in client-side Blazor, inject the `ILocalStorageService` per the example below.

```c#
@inject Blazored.LocalStorage.ILocalStorageService localStorage

@code {

    protected override async Task OnInitAsync()
    {
        await localStorage.SetItemAsync("name", "John Smith");
        var name = await localStorage.GetItemAsync<string>("name");
    }

}
```

With client-side Blazor you also have the option of a synchronous API, if your use case requires it. You can swap the `ILocalStorageService` for `ISyncStorageService` which allows you to avoid use of `async`/`await`. For either interface, the method names are the same.

```c#
@inject Blazored.LocalStorage.ISyncStorageService localStorage

@code {

    protected override void OnInit()
    {
        localStorage.SetItem("name", "John Smith");
        var name = localStorage.GetItem<string>("name");
    }

}
```

### Usage (Server-side Blazor)

**NOTE:** Due to pre-rendering in server-side Blazor you can't perform any JS interop until the `OnAfterRender` lifecycle method.

```c#
@inject Blazored.LocalStorage.ILocalStorageService localStorage

@functions {

    protected override async Task OnAfterRenderAsync()
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
- synchronous via `ISyncStorageService`:
  - SetItem()
  - GetItem()
  - RemoveItem()
  - Clear()
  - Length()
  - Key()

**Note:** Blazored.LocalStorage methods will handle the serialisation and de-serialisation of the data for you.
