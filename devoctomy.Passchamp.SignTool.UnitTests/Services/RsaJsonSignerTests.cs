﻿using devoctomy.Passchamp.SignTool.Services;
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
    public class RsaJsonSignerTests
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
            var path = $"Output/{Guid.NewGuid().ToString()}";
            await File.WriteAllTextAsync(
                path,
                testObjectJson);
            var sut = new RsaJsonSigner();

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
            var path = $"Output/{Guid.NewGuid().ToString()}";
            await File.WriteAllTextAsync(
                path,
                "POP!");
            var sut = new RsaJsonSigner();

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
            var path = $"Output/{Guid.NewGuid().ToString()}";
            await File.WriteAllTextAsync(
                path,
                testObjectJson);
            var sut = new RsaJsonSigner();

            using var rsaProvider = new RSACryptoServiceProvider();
            var privateKey = rsaProvider.ExportParameters(true);

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
            var sut = new RsaJsonSigner();

            using var rsaProvider = new RSACryptoServiceProvider();
            var privateKey = rsaProvider.ExportParameters(true);

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
