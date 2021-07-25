using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void GivenServiceCollection_WhenAddPasschampCoreServices_ThenServicesAdded()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddPasschampCoreServices();

            // Assert
            var provider = serviceCollection.BuildServiceProvider();
            Assert.NotNull(provider.GetService<IGraphLoaderService>());
            Assert.NotNull(provider.GetService<INodesJsonParserService>());
            Assert.NotNull(provider.GetService<IPinsJsonParserService>());
        }
    }
}
