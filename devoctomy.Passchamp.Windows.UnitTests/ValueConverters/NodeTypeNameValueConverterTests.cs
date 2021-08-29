using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Windows.ValueConverters;
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
            var allNodeTypes = typeof(INode).Assembly.GetTypes().Where(x => typeof(INode).IsAssignableFrom(x) && ! x.IsInterface).ToList();
            foreach(var curNodeType in allNodeTypes)
            {
                var node = Activator.CreateInstance(curNodeType);
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
