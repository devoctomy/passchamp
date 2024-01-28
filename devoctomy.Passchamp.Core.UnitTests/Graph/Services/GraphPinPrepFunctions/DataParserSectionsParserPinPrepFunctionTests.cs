using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Console;
using devoctomy.Passchamp.Core.Graph.Data;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Graph.Services.GraphPinPrepFunctions;
using System;
using System.Collections.Generic;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services.GraphPinPrepFunctions;

public class DataParserSectionsParserPinPrepFunctionTests
{
    [Theory]
    [InlineData("ParseDataParserSections", true)]
    [InlineData("Pop", false)]
    public void GivenKey_WhenIsApplicable_ThenCorrectValueReturned(
        string key,
        bool expectedResult)
    {
        // Arrange
        var sut = new DataParserSectionsParserPinPrepFunction(null);

        // Act
        var result = sut.IsApplicable(key);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GivenWrongCurNodeKey_AndValue_AndInputPins_AndNodes_WhenExecute_ThenNodeNotFound_AndKeyNotFoundExceptionThrown()
    {
        // Arrange
        var curNodeKey = "wrongkey";
        var value = "ParseDataParserSections.Pins.DataParserSections";
        var inputPins = new Dictionary<string, IPin>
        {
            { "DataParserSections", (IDataPin<string>)DataPinFactory.Instance.Create("DataParserSections", "Iv,0,16;Cipher,16,~16;Salt,~16,~0") }
        };
        var dataParserNode = new DataParserNode();
        var nodes = new Dictionary<string, INode>
        {
            { "dataparser", dataParserNode }
        };
        var sut = new DataParserSectionsParserPinPrepFunction(new DataParserSectionParser());

        // Act / Assert
        Assert.ThrowsAny<KeyNotFoundException>(() =>
        {
            _ = sut.Execute(
                curNodeKey,
                value,
                inputPins,
                nodes);
        });
    }

    [Fact]
    public void GivenCurNodeKey_AndWrongNodeType_AndValue_AndInputPins_AndNodes_WhenExecute_ThenNodeNotFound_AndInvalidOperationExceptionThrown()
    {
        // Arrange
        var curNodeKey = "dataparser";
        var value = "ParseDataParserSections.Pins.DataParserSections";
        var inputPins = new Dictionary<string, IPin>
        {
            { "DataParserSections", (IDataPin<string>)DataPinFactory.Instance.Create("DataParserSections", "Iv,0,16;Cipher,16,~16;Salt,~16,~0") }
        };
        var dataParserNode = new ConsoleInputNode();
        var nodes = new Dictionary<string, INode>
        {
            { "dataparser", dataParserNode }
        };
        var sut = new DataParserSectionsParserPinPrepFunction(new DataParserSectionParser());

        // Act / Assert
        Assert.ThrowsAny<InvalidOperationException>(() =>
        {
            _ = sut.Execute(
                curNodeKey,
                value,
                inputPins,
                nodes);
        });
    }

    [Fact]
    public void GivenCurNodeKey_AndValue_AndInputPins_AndNodes_WhenExecute_ThenNullReturned_AndDataParserSectionsSet()
    {
        // Arrange
        var curNodeKey = "dataparser";
        var value = "ParseDataParserSections.Pins.DataParserSections";
        var inputPins = new Dictionary<string, IPin>
        {
            { "DataParserSections", (IDataPin<string>)DataPinFactory.Instance.Create("DataParserSections", "Iv,0,16;Cipher,16,~16;Salt,~16,~0") }
        };
        var dataParserNode = new DataParserNode();
        var nodes = new Dictionary<string, INode>
        {
            { "dataparser", dataParserNode }
        };
        var sut = new DataParserSectionsParserPinPrepFunction(new DataParserSectionParser());

        // Act
        var result = sut.Execute(
            curNodeKey,
            value,
            inputPins,
            nodes);

        // Assert
        Assert.Null(result);
        Assert.NotNull(dataParserNode.Sections);
        Assert.Equal(3, dataParserNode.Sections.Value.Count);
        Assert.Equal("Iv", dataParserNode.Sections.Value[0].Key);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[0].Start.Offset);
        Assert.Equal(0, dataParserNode.Sections.Value[0].Start.Count);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[0].End.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[0].End.Count);

        Assert.Equal("Cipher", dataParserNode.Sections.Value[1].Key);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[1].Start.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[1].Start.Count);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[1].End.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[1].End.Count);

        Assert.Equal("Salt", dataParserNode.Sections.Value[2].Key);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[2].Start.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[2].Start.Count);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[2].End.Offset);
        Assert.Equal(0, dataParserNode.Sections.Value[2].End.Count);
    }

    [Fact]
    public void GivenCurNodeKey_AndRawValue_AndInputPins_AndNodes_WhenExecute_ThenNullReturned_AndDataParserSectionsSet()
    {
        // Arrange
        var curNodeKey = "dataparser";
        var value = "ParseDataParserSections.Iv,0,16;Cipher,16,~16;Salt,~16,~0";
        var dataParserNode = new DataParserNode();
        var nodes = new Dictionary<string, INode>
        {
            { "dataparser", dataParserNode }
        };
        var sut = new DataParserSectionsParserPinPrepFunction(new DataParserSectionParser());

        // Act
        var result = sut.Execute(
            curNodeKey,
            value,
            null,
            nodes);

        // Assert
        Assert.Null(result);
        Assert.NotNull(dataParserNode.Sections);
        Assert.Equal(3, dataParserNode.Sections.Value.Count);
        Assert.Equal("Iv", dataParserNode.Sections.Value[0].Key);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[0].Start.Offset);
        Assert.Equal(0, dataParserNode.Sections.Value[0].Start.Count);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[0].End.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[0].End.Count);

        Assert.Equal("Cipher", dataParserNode.Sections.Value[1].Key);
        Assert.Equal(Offset.Absolute, dataParserNode.Sections.Value[1].Start.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[1].Start.Count);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[1].End.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[1].End.Count);

        Assert.Equal("Salt", dataParserNode.Sections.Value[2].Key);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[2].Start.Offset);
        Assert.Equal(16, dataParserNode.Sections.Value[2].Start.Count);
        Assert.Equal(Offset.FromEnd, dataParserNode.Sections.Value[2].End.Offset);
        Assert.Equal(0, dataParserNode.Sections.Value[2].End.Count);
    }
}
