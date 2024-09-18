using Blazored.LocalStorage;
using Blazored.LocalStorage.Serialization;
using BlazorServer;
using InteractiveServer.Components;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Replace(ServiceDescriptor.Scoped<IJsonSerializer, NewtonSoftJsonSerializer>());

builder.Services.AddBlazoredLocalStorage();
// Use the below to enable streaming of objects with local storage
//builder.Services.AddBlazoredLocalStorageStreaming();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
