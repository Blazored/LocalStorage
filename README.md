# BlazoredLocalStorage
A library to provide access to local storage in Blazor applications

[![NuGet](https://img.shields.io/nuget/v/BlazoredLocalStorage.svg?style=flat-square)](https://www.nuget.org/packages/BlazoredLocalStorage/)

[![Build Status](https://dev.azure.com/chrissainty/BlazoredLocalStorage/_apis/build/status/chrissainty.BlazoredLocalStorage?branchName=master)](https://dev.azure.com/chrissainty/BlazoredLocalStorage/_build/latest?definitionId=2?branchName=master)

### Installing

You can install from Nuget using the following command:

`Install-Package BlazoredLocalStorage`

Or via the Visual Studio package manger.

### Setup

First, you will need to register local storage with the service collection in your _startup.cs_ file

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddLocalStorage();
}
``` 

### Usage
This is an example of using local storage in a .cshtml file 

```c#
@inject Blazored.Storage.ILocalStorage localStorage

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
