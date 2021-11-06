using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Console;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Services.GraphPinPrepFunctions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services.GraphPinPrepFunctions
{
    public class DataParserSectionGetterPinPrepFunctionTests
    {
        [Theory]
        [InlineData("GetDataParserSectionValue", true)]
        [InlineData("Pop", false)]
        public void GivenKey_WhenIsApplicable_ThenCorrectValueReturned(
            string key,
            bool expectedResult)
        {
            // Arrange
            var sut = new DataParserSectionGetterPinPrepFunction();

            // Act
            var result = sut.IsApplicable(key);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GivenValueWithWrongNodeName_AndNodeList_WhenExecute_ThenKeyNotFoundExceptionThrown()
        {
            // Arrange
            var value = "GetDataParserSectionValue.fruit.orange";
            var dataParserNode = new DataParserNode();
            dataParserNode.AttachGraph(Mock.Of<IGraph>());
            dataParserNode.Bytes = (IDataPin<byte[]>)DataPinFactory.Instance.Create("Bytes", new byte[] { 1, 1, 1, 1, 2, 2, 2, 2 });
            dataParserNode.Sections = (IDataPin<List<DataParserSection>>)DataPinFactory.Instance.Create(
                "Sections",
                new List<DataParserSection>
                {
                    new DataParserSection
                    {
                        Key = "apple",
                        Start = new ArrayLocation(Offset.Absolute, 0),
                        End = new ArrayLocation(Offset.Absolute, 3)
                    },
                    new DataParserSection
                    {
                        Key = "orange",
                        Start = new ArrayLocation(Offset.Absolute, 4),
                        End = new ArrayLocation(Offset.Absolute, 8)
                    }
                });
            var nodes = new Dictionary<string, INode>
            {
                { "dataparser", dataParserNode }
            };
            await dataParserNode.ExecuteAsync(CancellationToken.None);

            var sut = new DataParserSectionGetterPinPrepFunction();

            // Act
            Assert.ThrowsAny<KeyNotFoundException>(() =>
            {
                var result = sut.Execute(
                    null,
                    value,
                    null,
                    nodes);
            });
        }

        [Fact]
        public void GivenValue_AndWrongNodeType_AndNodeList_WhenExecute_ThenInvalidOperationExceptionThrown()
        {
            // Arrange
            var value = "GetDataParserSectionValue.dataparser.orange";
            var dataParserNode = new ConsoleInputNode();
            dataParserNode.AttachGraph(Mock.Of<IGraph>());
            var nodes = new Dictionary<string, INode>
            {
                { "dataparser", dataParserNode }
            };

            var sut = new DataParserSectionGetterPinPrepFunction();

            // Act
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                sut.Execute(
                    null,
                    value,
                    null,
                    nodes);
            });
        }

        [Fact]
        public async Task GivenValue_AndNodeList_WhenExecute_ThenSectionDataPinReturned()
        {
            // Arrange
            var value = "GetDataParserSectionValue.dataparser.orange";
            var dataParserNode = new DataParserNode();
            dataParserNode.AttachGraph(Mock.Of<IGraph>());
            dataParserNode.Bytes = (IDataPin<byte[]>)DataPinFactory.Instance.Create("Bytes", new byte[] { 1, 1, 1, 1, 2, 2, 2, 2 });
            dataParserNode.Sections = (IDataPin<List<DataParserSection>>)DataPinFactory.Instance.Create(
                "Sections",
                new List<DataParserSection>
                {
                    new DataParserSection
                    {
                        Key = "apple",
                        Start = new ArrayLocation(Offset.Absolute, 0),
                        End = new ArrayLocation(Offset.Absolute, 3)
                    },
                    new DataParserSection
                    {
                        Key = "orange",
                        Start = new ArrayLocation(Offset.Absolute, 4),
                        End = new ArrayLocation(Offset.Absolute, 8)
                    }
                });
            var nodes = new Dictionary<string, INode>
            {
                { "dataparser", dataParserNode }
            };
            await dataParserNode.ExecuteAsync(CancellationToken.None);

            var sut = new DataParserSectionGetterPinPrepFunction();

            // Act
            var result = sut.Execute(
                null,
                value,
                null,
                nodes);

            // Assert
            Assert.Equal("orange", result.Name);
            Assert.Equal(result.ObjectValue, new byte[] { 2, 2, 2, 2 });
        }

    }
}
