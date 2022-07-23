using devoctomy.Passchamp.SignTool.Services;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class JsonWebKeyRsaKeyGeneratorServiceTests
    {
        [Theory]
        [InlineData(2048)]
        [InlineData(4096)]
        public void GivenKeyTypeRsaJsonWebKey_AndKeySize_WhenGenerate_ThenKeyPairGenerated_AndKeyPairValid(
            int keySize)
        {
            // Arrange
            var sut = new JsonWebKeyRsaKeyGeneratorService();

            // Act
            sut.Generate(
                keySize,
                out var privateKey,
                out var publicKey);

            // Assert
            var privateJWK = new JsonWebKey(privateKey);
            var publicJWK = new JsonWebKey(publicKey);

            Assert.True(privateJWK.HasPrivateKey);
            Assert.Equal(keySize, privateJWK.KeySize);
            Assert.Equal($"RSA-{keySize}-Sign", privateJWK.Alg);
            Assert.True(privateJWK.CanComputeJwkThumbprint());

            Assert.False(publicJWK.HasPrivateKey);
            Assert.Equal(keySize, publicJWK.KeySize);
            Assert.Equal($"RSA-{keySize}-Sign", publicJWK.Alg);
            Assert.True(publicJWK.CanComputeJwkThumbprint());
        }
    }
}
