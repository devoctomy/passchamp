using devoctomy.Passchamp.SignTool.Exceptions;
using devoctomy.Passchamp.SignTool.Services;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class RsaJsonVerifierTests
    {
        [Fact]
        public async Task GivenPath_AndValidJson_WhenIsApplicable_ThenTrueReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifier();

            // Act
            var result = await sut.IsApplicable("Data/ValidSignedJson.json");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GivenPath_AndInvalidAlgorithm_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifier();

            // Act
            var result = await sut.IsApplicable("Data/InvalidAlgorithmSignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndMissingSignature_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifier();

            // Act
            var result = await sut.IsApplicable("Data/MissingSignatureSignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndUnsignedJson_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifier();

            // Act
            var result = await sut.IsApplicable("Data/UnsignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndValidSignature_AndPublicKey_WhenVerify_ThenNoExceptionThrown()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var publicKey = JsonConvert.DeserializeObject<RSAParameters>(publicKeyText);
            var sut = new RsaJsonVerifier();

            // Act & Assert
            await sut.Verify(
                "Data/ValidSignedJson.json",
                publicKey);
        }

        [Fact]
        public async Task GivenPath_AndTampered_AndPublicKey_WhenVerify_ThenNoExceptionThrown()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var publicKey = JsonConvert.DeserializeObject<RSAParameters>(publicKeyText);
            var sut = new RsaJsonVerifier();

            // Act & Assert
            await Assert.ThrowsAnyAsync<InvalidSignatureException>(async () => 
            {
                await sut.Verify(
                    "Data/TamperedSignedJson.json",
                    publicKey);
            });
        }

        [Fact]
        public async Task GivenPath_AndInvalidSignature_AndPublicKey_WhenVerify_ThenNoExceptionThrown()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var publicKey = JsonConvert.DeserializeObject<RSAParameters>(publicKeyText);
            var sut = new RsaJsonVerifier();

            // Act & Assert
            await Assert.ThrowsAnyAsync<InvalidSignatureException>(async () =>
            {
                await sut.Verify(
                    "Data/InvalidSignatureSignedJson.json",
                    publicKey);
            });
        }
    }
}
