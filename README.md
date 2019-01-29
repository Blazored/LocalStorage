# Blazored LocalStorage
A library to provide access to local storage in Blazor applications

### Installing

You can install from Nuget using the following command:

`Install-Package Blazored.LocalStorage`

Or via the Visual Studio package manger.

### Setup

First, you will need to register local storage with the service collection in your _startup.cs_ file

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddBlazoredLocalStorage();
}
``` 

### Usage
This is an example of using local storage in a .cshtml file 

```c#
@inject Blazored.LocalStorage.ILocalStorageService localStorage

@functions {

    protected override async Task OnInitAsync()
    {
        await localStorage.SetItem("name", "John Smith");
        var name = await localStorage.GetItem<string>("name");
    }

}
```

The APIs available are
 - SetItem()
 - GetItem()
 - RemoveItem()
 - Clear()
 - Length()
 - Key()

 **All APIs are now _async_**

**Note:** Blazored.LocalStorage methods will handle the serialisation and de-serialisation of the data for you.
