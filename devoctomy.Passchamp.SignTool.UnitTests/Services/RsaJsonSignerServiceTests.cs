using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.UnitTests.Test;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services;

public class RsaJsonSignerServiceTests
{
    [Fact]
    public async Task GivenPath_AndValidJson_WhenIsApplicable_ThenTrueReturned()
    {
        // Arrange
        var testObject = new SimpleObject
        {
            Name = "Bob Hoskins",
            Age = 100
        };
        var testObjectJson = JsonConvert.SerializeObject(testObject, Formatting.Indented);
        var path = $"Output/{Guid.NewGuid()}";
        await File.WriteAllTextAsync(
            path,
            testObjectJson);
        var sut = new RsaJsonSignerService();

        // Act
        var result = await sut.IsApplicable(path);

        // Assert
        Assert.True(result);

        // Cleanup
        File.Delete(path);
    }

    [Fact]
    public async Task GivenPath_AndInvalidJson_WhenIsApplicable_ThenFalseReturned()
    {
        // Arrange
        var path = $"Output/{Guid.NewGuid()}";
        await File.WriteAllTextAsync(
            path,
            "POP!");
        var sut = new RsaJsonSignerService();

        // Act
        var result = await sut .IsApplicable(path);

        // Assert
        Assert.False(result);

        // Cleanup
        File.Delete(path);
    }

    [Fact]
    public async Task GivenSignOptions_WhenSign_ThenJsonSigned_AndSignatureAdded()
    {
        // Arrange
        var testObject = new SimpleObject
        {
            Name = "Bob Hoskins",
            Age = 100
        };
        var testObjectJson = JsonConvert.SerializeObject(testObject, Formatting.Indented);
        var path = $"Output/{Guid.NewGuid()}";
        await File.WriteAllTextAsync(
            path,
            testObjectJson);
        var keyGen = new RsaKeyGeneratorService();
        var keyPair = keyGen.Generate(1024);
        var keyFile = $"Output/{Guid.NewGuid()}";
        await File.WriteAllTextAsync(
            keyFile,
            keyPair.PrivateKey);
        var sut = new RsaJsonSignerService();
        var output = $"Output/{Guid.NewGuid()}";

        // Act
        var result = await sut.Sign(new SignOptions
        {
            Input = path,
            KeyFile = keyFile,
            Output = output
        });

        // Assert
        Assert.Equal(0, result);
        var signedResult = await File.ReadAllTextAsync(output);
        var signedJson = JObject.Parse(signedResult);
        Assert.True(signedJson.ContainsKey("Signature"));
        Assert.Equal("RsaJsonSigner", signedJson["Signature"]["Algorithm"].Value<string>());
        Assert.False(String.IsNullOrEmpty(signedJson["Signature"]["Signature"].Value<string>()));

        // Cleanup
        File.Delete(path);
        File.Delete(keyFile);
        File.Delete(output);
    }

    [Fact]
    public async Task GivenPath_AndValidJson_AndKey_WhenSign_ThenJsonSigned_AndSignatureAdded()
    {
        // Arrange
        var testObject = new SimpleObject
        {
            Name = "Bob Hoskins",
            Age = 100
        };
        var testObjectJson = JsonConvert.SerializeObject(testObject, Formatting.Indented);
        var path = $"Output/{Guid.NewGuid()}";
        await File.WriteAllTextAsync(
            path,
            testObjectJson);
        var keyGen = new RsaKeyGeneratorService();
        var keyPair = keyGen.Generate(1024);
        var sut = new RsaJsonSignerService();

        // Act
        var signedResult = await sut.Sign(
            path,
            keyPair.PrivateKey);

        // Assert
        var signedJson = JObject.Parse(signedResult);
        Assert.True(signedJson.ContainsKey("Signature"));
        Assert.Equal("RsaJsonSigner", signedJson["Signature"]["Algorithm"].Value<string>());
        Assert.False(String.IsNullOrEmpty(signedJson["Signature"]["Signature"].Value<string>()));

        // Cleanup
        File.Delete(path);
    }

    [Fact]
    public async Task GivenPath_AndValidJson_AndKey_AndJsonAlreadySigned_WhenSign_ThenJsonSigned_AndSignatureAdded()
    {
        // Arrange
        var keyGen = new RsaKeyGeneratorService();
        var keyPair = keyGen.Generate(1024);
        var sut = new RsaJsonSignerService();

        // Act
        var signedResult = await sut.Sign(
            "Data/ValidSignedJson.json",
            keyPair.PrivateKey);

        // Assert
        var signedJson = JObject.Parse(signedResult);
        Assert.True(signedJson.ContainsKey("Signature"));
        Assert.Equal("RsaJsonSigner", signedJson["Signature"]["Algorithm"].Value<string>());
        Assert.False(String.IsNullOrEmpty(signedJson["Signature"]["Signature"].Value<string>()));
    }
}
