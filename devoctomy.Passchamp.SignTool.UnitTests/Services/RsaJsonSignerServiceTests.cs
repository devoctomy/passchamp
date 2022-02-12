using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.UnitTests.Test;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
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
                testObjectJson).ConfigureAwait(false);
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
            keyGen.Generate(
                1024,
                out var privateKey,
                out _);
            var sut = new RsaJsonSignerService();

            using var rsaProvider = new RSACryptoServiceProvider();

            // Act
            var signedResult = await sut.Sign(
                path,
                privateKey);

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
            keyGen.Generate(
                1024,
                out var privateKey,
                out _);
            var sut = new RsaJsonSignerService();

            using var rsaProvider = new RSACryptoServiceProvider();

            // Act
            var signedResult = await sut.Sign(
                "Data/ValidSignedJson.json",
                privateKey);

            // Assert
            var signedJson = JObject.Parse(signedResult);
            Assert.True(signedJson.ContainsKey("Signature"));
            Assert.Equal("RsaJsonSigner", signedJson["Signature"]["Algorithm"].Value<string>());
            Assert.False(String.IsNullOrEmpty(signedJson["Signature"]["Signature"].Value<string>()));
        }
    }
}
