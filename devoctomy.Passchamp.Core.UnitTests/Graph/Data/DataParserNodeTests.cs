﻿using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Data;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Data;

public class DataParserNodeTests
{
    [Fact]
    public async Task GivenBytes_AndSections_WhenExecute_ThenSectionValuesParsed_AndNextExecuted()
    {
        // Arrange
        var sut = new DataParserNode
        {
            Bytes = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Bytes",
                new byte[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3 }),
            Sections = (IDataPin<List<DataParserSection>>)DataPinFactory.Instance.Create(
                "Sections",
                new List<DataParserSection>
                {
                    new DataParserSection
                    {
                        Key = "Start",
                        Start = new ArrayLocation(Offset.Absolute, 0),
                        End = new ArrayLocation(Offset.Absolute, 4),
                    },
                    new DataParserSection
                    {
                        Key = "Middle",
                        Start = new ArrayLocation(Offset.Absolute, 4),
                        End = new ArrayLocation(Offset.FromEnd, 4),
                    },
                    new DataParserSection
                    {
                        Key = "End",
                        Start = new ArrayLocation(Offset.FromEnd, 4),
                        End = new ArrayLocation(Offset.FromEnd, 0),
                    }
                },
                typeof(List<DataParserSection>)),
            NextKey = "hello"
        };
        var mockGraph = new Mock<IGraph>();
        var mockNextNode = new Mock<INode>();
        mockGraph.Setup(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)))
            .Returns(mockNextNode.Object);
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act
        await sut.ExecuteAsync(cancellationTokenSource.Token);

        // Assert
        Assert.Equal(new byte[] { 1, 1, 1, 1 }, sut.GetSectionValue("Start").Value);
        Assert.Equal(new byte[] { 2, 2, 2, 2 }, sut.GetSectionValue("Middle").Value);
        Assert.Equal(new byte[] { 3, 3, 3, 3 }, sut.GetSectionValue("End").Value);
        mockGraph.Verify(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)), Times.Once);
        mockNextNode.Verify(x => x.ExecuteAsync(It.Is<CancellationToken>(x => x == cancellationTokenSource.Token)), Times.Once);
    }
}
