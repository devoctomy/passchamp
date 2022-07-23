using devoctomy.Passchamp.SignTool.Services;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class JsonWebKeyRsaKeyGeneratorServiceTests
    {
        [Theory]
        [InlineData(2048, 2035, 851)]
        [InlineData(4096, 3572, 1192)]
        public void GivenKeyTypeRsaJsonWebKey_AndKeySize_WhenGenerate_ThenKeyPairGenerated_AndKeysCorrectSize_AndKeyPairValid(
            int keySize,
            int privateKeyParamsSize,
            int publicKeyParamsSize)
        {
            // Arrange
            var sut = new JsonWebKeyRsaKeyGeneratorService();

            // Act
            sut.Generate(
                keySize,
                out var privateKey,
                out var publicKey);

            // Assert
            Assert.Equal(privateKeyParamsSize, privateKey.Length);
            Assert.Equal(publicKeyParamsSize, publicKey.Length);
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
