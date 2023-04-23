using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.LocalStorage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredLocalStorage(this IServiceCollection services)
            => AddBlazoredLocalStorage(services, null);

        public static IServiceCollection AddBlazoredLocalStorage(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, BrowserStorageProvider>()
                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddScoped<ISyncLocalStorageService, LocalStorageService>()
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
        
        /// <summary>
        /// Registers the Blazored LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredLocalStorageAsSingleton(this IServiceCollection services)
            => AddBlazoredLocalStorageAsSingleton(services, null);
        
        /// <summary>
        /// Registers the Blazored LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredLocalStorageAsSingleton(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            return services
                .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                .AddSingleton<IStorageProvider, BrowserStorageProvider>()
                .AddSingleton<ILocalStorageService, LocalStorageService>()
                .AddSingleton<ISyncLocalStorageService, LocalStorageService>()
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}
