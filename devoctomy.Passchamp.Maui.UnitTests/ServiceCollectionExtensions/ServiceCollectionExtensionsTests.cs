﻿using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using Moq;

namespace devoctomy.Passchamp.Maui.UnitTests.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void GivenServiceCollection_WhenAddPasschampMauiServices_ThenServicesAdded()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(Mock.Of<ISecureStorage>());

        // Act
        serviceCollection.AddPasschampMauiServices(new PasschampMauiServicesOptions
        {
            VaultLoaderServiceOptions = new Maui.Data.VaultLoaderServiceOptions(),
            ShellNavigationServiceOptions = new Services.ShellNavigationServiceOptions(),
            ThemeAwareImageResourceServiceOptions = new Services.ThemeAwareImageResourceServiceOptions(),
        });

        // Assert
        var provider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(provider.GetService<ISecureSettingStorageService>());
    }
}
