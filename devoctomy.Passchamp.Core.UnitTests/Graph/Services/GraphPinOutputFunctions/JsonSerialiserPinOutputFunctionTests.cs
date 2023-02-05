using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Services.GraphPinOutputFunctions;
using devoctomy.Passchamp.Core.Graph.Vault;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services.GraphPinOutputFunctions;

public class JsonSerialiserPinOutputFunctionTests
{
    [Theory]
    [InlineData("JsonSerialiserPinOutputFunction", true)]
    [InlineData("Pop", false)]
    public void GivenKey_WhenIsApplicable_ThenCorrectValueReturned(
        string key,
        bool expectedResult)
    {
        // Arrange
        var sut = new JsonSerialiserPinOutputFunction();

        // Act
        var result = sut.IsApplicable(key);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GivenValueWithWrongNodeName_AndNodeList_WhenExecute_ThenKeyNotFoundExceptionThrown()
    {
        // Arrange
        var value = "JsonSerialiserPinOutputFunction.wrongname.Vault";
        var vaultParserNode = new VaultParserNode();
        vaultParserNode.Output["Vault"] = (IDataPin<Core.Vault.Vault>)DataPinFactory.Instance.Create(
            "Sections",
            new Core.Vault.Vault
            {
                Id = "Hello World!"
            });

        var sut = new JsonSerialiserPinOutputFunction();
        var nodes = new Dictionary<string, INode>
        {
            { "parser", vaultParserNode }
        };

        // Act & Assert
        Assert.ThrowsAny<KeyNotFoundException>(() =>
        {
            sut.Execute(
                value,
                nodes);
        });
    }

    [Fact]
    public void GivenValue_AndNodeList_WhenExecute_ThenCorrectValueSet()
    {
        // Arrange
        var value = "JsonSerialiserPinOutputFunction.parser.Vault";
        var vaultParserNode = new VaultParserNode();
        vaultParserNode.Output["Vault"] = (IDataPin<Core.Vault.Vault>)DataPinFactory.Instance.Create(
            "Sections",
            new Core.Vault.Vault
            {
                Id = "Hello World!"
            });

        var sut = new JsonSerialiserPinOutputFunction();
        var nodes = new Dictionary<string, INode>
        {
            { "parser", vaultParserNode }
        };

        // Act
        var result = sut.Execute(
            value,
            nodes);

        // Assert
        Assert.Equal("Value", result.Name);
        Assert.IsType<string>(result.ObjectValue);
        var resultJson = result.ObjectValue as string;
        var expectedJson = JsonConvert.SerializeObject(
            vaultParserNode.Output["Vault"].ObjectValue,
            Formatting.Indented);
        Assert.Equal(expectedJson, resultJson);
    }
}
