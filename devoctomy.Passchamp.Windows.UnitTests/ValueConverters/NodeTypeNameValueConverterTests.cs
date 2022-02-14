using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.ValueConverters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace devoctomy.Passchamp.Windows.UnitTests.ValueConverters
{
    public class NodeTypeNameValueConverterTests
    {
        [Fact]
        public void GivenAnyCoreNode_WhenConvert_ThenTypeNameReturned()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddPasschampCoreServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var allNodeTypes = typeof(INode).Assembly.GetTypes().Where(x => typeof(INode).IsAssignableFrom(x) && ! x.IsInterface).ToList();
            foreach(var curNodeType in allNodeTypes)
            {
                var node = serviceProvider.GetService(curNodeType);
                var sut = new NodeTypeNameValueConverter();

                // Act
                var result = sut.Convert(
                    node,
                    null,
                    null,
                    null);

                // Assert
                Assert.Equal(curNodeType.Name, result);
            }
        }
    }
}
