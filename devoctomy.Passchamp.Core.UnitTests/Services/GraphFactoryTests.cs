using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services;

public class GraphFactoryTests
{
    [Fact]
    public async Task GivenParameters_WhenLoadNativeEncryptDefault_ThenGraphCreated_AndGraphExecutesSuccessfully()
    {
        // Arrange
        var outputDirName = Guid.NewGuid().ToString();
        var outputFileName = $"{outputDirName}/DefaultNativeEncrypt.enc";
        var sut = new GraphFactory(new SecureStringUnpacker());
        var parameters = new Dictionary<string, object>
        {
            { "passphrase", "password123" },
            { "outputfilename", outputFileName },
            { "plaintext", "Hello World!" },
        };

        // Act
        var graph = sut.LoadNative(
            Enums.GraphContext.Encrypt,
            Enums.NativeGraphs.Default,
            [.. parameters]);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
    }

}
