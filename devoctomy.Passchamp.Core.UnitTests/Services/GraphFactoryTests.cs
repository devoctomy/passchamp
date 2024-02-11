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
    public async Task GivenNativeDefaultEncryptPreset_AndParameters_WhenLoadPreset_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputStreamContainsData()
    {
        // Arrange
        var outputStream = new MemoryStream();
        var sut = new GraphFactory();
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
    public async Task GivenNativeDefaultDecryptPreset_AndParameters_WhenLoadPreset_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputContainsCorrectData()
    {
        // Arrange
        var inputStream = new MemoryStream(Convert.FromBase64String("BWnAbP+4WxKU1PgZjkb6l//xlo3PEqQOXjrwcVmjLMf11CuQwg/+CSmEIuBWzQ54"));
        var sut = new GraphFactory();
        var parameters = new Dictionary<string, object>
        {
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "InputStream", inputStream }
        };
        var nativeDefaultDecryptPreset = NativePresets.DefaultDecrypt();

        // Act
        var graph = sut.LoadPreset(nativeDefaultDecryptPreset, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        var plainText = System.Text.Encoding.UTF8.GetString((byte[])graph.OutputPins["DecryptedBytes"].ObjectValue);
        Assert.Equal("Hello World!", plainText);
    }

    [Fact]
    public async Task GivenEncyptContext_AndDefaultNativeGraph_AndParameters_WhenLoadNative_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputStreamContainsData()
    {
        // Arrange
        var outputStream = new MemoryStream();
        var sut = new GraphFactory();
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

    [Fact]
    public async Task GivenDecryptContext_AndDefaultNativeGraph_AndParameters_WhenLoadNative_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputContainsCorrectData()
    {
        // Arrange
        var inputStream = new MemoryStream(Convert.FromBase64String("BWnAbP+4WxKU1PgZjkb6l//xlo3PEqQOXjrwcVmjLMf11CuQwg/+CSmEIuBWzQ54"));
        var sut = new GraphFactory();
        var parameters = new Dictionary<string, object>
        {
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "InputStream", inputStream }
        };

        // Act
        var graph = sut.LoadNative(Enums.GraphContext.Decrypt, Enums.NativeGraphs.Default, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        var plainText = System.Text.Encoding.UTF8.GetString((byte[])graph.OutputPins["DecryptedBytes"].ObjectValue);
        Assert.Equal("Hello World!", plainText);
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
