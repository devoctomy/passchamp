using devoctomy.Passchamp.SignTool.Services;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services;

public class RsaKeyGeneratorServiceTests
{
    [Theory]
    [InlineData(1024, 867, 267)]
    [InlineData(2048, 1631, 439)]
    [InlineData(3072, 2387, 607)]
    [InlineData(4096, 3171, 779)]
    public void GivenKeyTypeJsonRsaParameters_AndKeySize_WhenGenerate_ThenKeyPairGenerated_AndKeysCorrectSize_AndKeyPairValid(
        int keySize,
        int privateKeyParamsSize,
        int publicKeyParamsSize)
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
        Assert.Equal(privateKeyParamsSize, privateKey.Length);
        Assert.Equal(publicKeyParamsSize, publicKey.Length);
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
