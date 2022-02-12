using devoctomy.Passchamp.Core.Graph.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services
{
    public class NodesJsonParserServiceTests
    {
        [Theory]
        [InlineData(
            "Data/nodes1.json",
            "node1",
            "node1,node2")]
        public async Task GivenJsonData_WhenParse_ThenNodesAndStartKeyReturned(
            string fileName,
            string expectedStartNodeKey,
            string expectedNodeKeys)
        {
            // Arrange
            var jsonData = await System.IO.File.ReadAllTextAsync(
                fileName,
                CancellationToken.None);
            var nodesJson = JObject.Parse(jsonData);
            var sut = new NodesJsonParserService();

            // Act
            var result = sut.Parse(
                nodesJson["Nodes"].Value<JArray>(),
                out var startNodeKey);

            // Assert
            var keys = result.Keys.ToArray();
            Assert.Equal(expectedStartNodeKey, startNodeKey);
            Assert.Equal(expectedNodeKeys, string.Join(",", keys));
        }

        [Theory]
        [InlineData("Data/nodes2.json")]
        public async Task GivenJsonData_AndTypesInvalid_WhenParse_ThenTypeLoadExceptionThrown(string fileName)
        {
            // Arrange
            var jsonData = await System.IO.File.ReadAllTextAsync(
                fileName,
                CancellationToken.None).ConfigureAwait(false);
            var nodesJson = JObject.Parse(jsonData);
            var sut = new NodesJsonParserService();

            // Act & Assert
            Assert.ThrowsAny<TypeLoadException>(() =>
            {
                sut.Parse(
                    nodesJson["Nodes"].Value<JArray>(),
                    out var startNodeKey);
            });
        }

        [Theory]
        [InlineData("Data/nodes3.json")]
        public async Task GivenJsonData_AndUnknownInputKey_WhenParse_ThenKeyNotFoundExceptionThrown(string fileName)
        {
            // Arrange
            var jsonData = await System.IO.File.ReadAllTextAsync(
                fileName,
                CancellationToken.None).ConfigureAwait(false);
            var nodesJson = JObject.Parse(jsonData);
            var sut = new NodesJsonParserService();

            // Act & Assert
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                var result = sut.Parse(
                    nodesJson["Nodes"].Value<JArray>(),
                    out var startNodeKey);
            });
        }
    }
}
