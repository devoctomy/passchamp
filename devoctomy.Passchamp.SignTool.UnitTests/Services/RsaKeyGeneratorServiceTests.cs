using devoctomy.Passchamp.SignTool.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class RsaKeyGeneratorServiceTests
    {
        [Theory]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(3072)]
        [InlineData(4096)]
        public void GivenKeySize_WhenGenerate_ThenKeyPairGenerated_AndKeyPairValid(int keySize)
        {
            // Arrange
            var plainBytes = new byte[] { 1, 2, 3, 4, 5, 6 };
            var sut = new RsaKeyGeneratorService();

            // Act
            sut.Generate(
                keySize,
                out var privateKey,
                out var publicKey);

            // Assert
            var privateKeyParams = JsonConvert.DeserializeObject<RSAParameters>(privateKey);
            var publicKeyParams = JsonConvert.DeserializeObject<RSAParameters>(publicKey);

            using var sha256Provider = SHA256.Create();
            using var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.ImportParameters(privateKeyParams);

            var signature = rsaProvider.SignData(
                plainBytes,
                sha256Provider);

            rsaProvider.ImportParameters(publicKeyParams);

            var result = rsaProvider.VerifyData(
                plainBytes,
                sha256Provider,
                signature);

            Assert.True(result);
        }
    }
}
