using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services;

public class GraphFactoryTests
{
    [Fact]
    public async Task GivenNativeDefaultEncryptPreset_AndParameters_WhenLoadLoadPreset_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputStreamContainsData()
    {
        // Arrange
        var outputStream = new MemoryStream();
        var sut = new GraphFactory(new SecureStringUnpacker());
        var parameters = new Dictionary<string, object>
        {
            { "SaltLength", 16 },
            { "IvLength", 16 },
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "OutputStream", outputStream },
            { "PlainText", "Hello World!" },
        };
        var nativeDefaultEncryptPreset = NativePresets.DefaultEncrypt();

        // Act
        var graph = sut.LoadPreset(nativeDefaultEncryptPreset, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        Assert.True(outputStream.Length > 0);
    }

    [Fact]
    public async Task GivenEncyptContext_AndDefaultNativeGraph_AndParameters_WhenLoadNative_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputStreamContainsData()
    {
        // Arrange
        var outputStream = new MemoryStream();
        var sut = new GraphFactory(new SecureStringUnpacker());
        var parameters = new Dictionary<string, object>
        {
            { "SaltLength", 16 },
            { "IvLength", 16 },
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "OutputStream", outputStream },
            { "PlainText", "Hello World!" },
        };

        // Act
        var graph = sut.LoadNative(Enums.GraphContext.Encrypt, Enums.NativeGraphs.Default, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        Assert.True(outputStream.Length > 0);
    }

    private INode InstantiateNode(Type type)
    {
        if (type == typeof(DeriveKeyFromPasswordExNode))
        {
            return new DeriveKeyFromPasswordExNode(new SecureStringUnpacker());
        }
        else
        {
            return Activator.CreateInstance(type) as INode;
        }  
    }
}
