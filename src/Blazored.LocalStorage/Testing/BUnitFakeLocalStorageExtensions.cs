using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blazored.LocalStorage.Testing
{
    public static class BUnitFakeLocalStorageExtensions
    {
        public static void AddBlazoredLocalStorage(this TestContextBase context)
        {
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));

            context.Services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, InMemoryStorageProvider>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<ISyncLocalStorageService, LocalStorageService>()
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }

        public static void AddBlazoredLocalStorage(this TestContextBase context, Action<LocalStorageOptions> configure)
        {
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));

            context.Services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, InMemoryStorageProvider>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<ISyncLocalStorageService, LocalStorageService>()
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });

        }
    }
}
