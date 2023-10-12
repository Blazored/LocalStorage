using System;
using System.Linq;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Blazored.LocalStorage.Tests.LocalStorageServiceTests;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void Scoped()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddBlazoredLocalStorage();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredLocalStorage();

        // Assert
        AssertEqual(services, typeof(IJsonSerializer), typeof(SystemTextJsonSerializer), ServiceLifetime.Scoped);
        AssertEqual(services, typeof(IStorageProvider), typeof(BrowserStorageProvider), ServiceLifetime.Scoped);
        AssertEqual(services, typeof(ILocalStorageService), typeof(LocalStorageService), ServiceLifetime.Scoped);
        AssertEqual(services, typeof(ISyncLocalStorageService), typeof(LocalStorageService), ServiceLifetime.Scoped);
        AssertEqualConfigureOptions(services, typeof(IConfigureOptions<LocalStorageOptions>), ServiceLifetime.Singleton);
    }

    [Fact]
    public void Singleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddBlazoredLocalStorageAsSingleton();
        services.AddBlazoredLocalStorageAsSingleton();
        services.AddBlazoredLocalStorageAsSingleton();
        services.AddBlazoredLocalStorageAsSingleton();

        // Assert
        AssertEqual(services, typeof(IJsonSerializer), typeof(SystemTextJsonSerializer), ServiceLifetime.Singleton);
        AssertEqual(services, typeof(IStorageProvider), typeof(BrowserStorageProvider), ServiceLifetime.Singleton);
        AssertEqual(services, typeof(ILocalStorageService), typeof(LocalStorageService), ServiceLifetime.Singleton);
        AssertEqual(services, typeof(ISyncLocalStorageService), typeof(LocalStorageService), ServiceLifetime.Singleton);
        AssertEqualConfigureOptions(services, typeof(IConfigureOptions<LocalStorageOptions>), ServiceLifetime.Singleton);
    }


    static void AssertEqual(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceLifetime serviceLifetime
        )
    {
        var descriptors = services.Where(serviceDescriptor => serviceDescriptor.ServiceType == serviceType).ToArray();
        Assert.Single(descriptors);
        var descriptor = descriptors.Single();
        Assert.Equal(serviceType, descriptor.ServiceType);
        Assert.Equal(implementationType, descriptor.ImplementationType);
        Assert.Equal(serviceLifetime, descriptor.Lifetime);
    }

    static void AssertEqualConfigureOptions(
        IServiceCollection services,
        Type serviceType,
        ServiceLifetime serviceLifetime
    )
    {
        var descriptors = services.Where(serviceDescriptor => serviceDescriptor.ServiceType == serviceType).ToArray();
        Assert.Single(descriptors);
        var descriptor = descriptors.Single();
        Assert.Equal(serviceType, descriptor.ServiceType);
        Assert.Equal(serviceLifetime, descriptor.Lifetime);
    }
}
