using devoctomy.Passchamp.SignTool.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class RsaJsonVerifierServiceTests
    {
        [Fact]
        public async Task GivenPath_AndValidJson_WhenIsApplicable_ThenTrueReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.IsApplicable("Data/ValidSignedJson.json");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GivenPath_AndInvalidAlgorithm_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.IsApplicable("Data/InvalidAlgorithmSignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndMissingSignature_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.IsApplicable("Data/MissingSignatureSignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndUnsignedJson_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.IsApplicable("Data/UnsignedJson.json");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndValidSignature_AndPublicKey_WhenVerify_ThenTrueReturned()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.Verify(
                "Data/ValidSignedJson.json",
                publicKeyText);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GivenAndVerifyOptions_AndValidSignature_WhenVerify_ThenZeroReturned()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var sut = new RsaJsonVerifierService();

            var verifyOptions = new VerifyOptions
            {
                KeyFile = "Data/PublicKey.json",
                Input = "Data/ValidSignedJson.json"
            };

            // Act
            var result = await sut.Verify(verifyOptions);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GivenAndVerifyOptions_AndTampered_WhenVerify_ThenErrorCodeReturned()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var sut = new RsaJsonVerifierService();

            var verifyOptions = new VerifyOptions
            {
                KeyFile = "Data/PublicKey.json",
                Input = "Data/TamperedSignedJson.json"
            };

            // Act
            var result = await sut.Verify(verifyOptions);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenPath_AndTampered_AndPublicKey_WhenVerify_ThenFalseReturned()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json");
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.Verify(
                    "Data/TamperedSignedJson.json",
                    publicKeyText);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GivenPath_AndInvalidSignature_AndPublicKey_WhenVerify_ThenFalseReturned()
        {
            // Arrange
            var publicKeyText = await File.ReadAllTextAsync("Data/PublicKey.json").ConfigureAwait(false);
            var sut = new RsaJsonVerifierService();

            // Act
            var result = await sut.Verify(
                    "Data/InvalidSignatureSignedJson.json",
                    publicKeyText);

            // Assert
            Assert.False(result);
        }
    }
}
