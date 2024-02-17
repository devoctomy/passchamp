using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Graph.Services;
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
    public async Task GivenNativeDefaultEncryptPresetGraph_AndNativeDefaultDecryptPresetGraph_WhenExecuteSequentially_ThenDataSuccessfullyEncrypted_AndDataSuccessfullyDecrypted()
    {
        // Arrange
        var plainText = "Hello World!";
        var password = "password123";
        using var outputStream = new MemoryStream();
        var sut = Instantiate();
        var encryptParameters = new Dictionary<string, object>
        {
            { "SaltLength", 16 },
            { "IvLength", 16 },
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, password).SecurePassword },
            { "OutputStream", outputStream },
            { "PlainText", plainText },
        };
        var nativeDefaultEncryptPreset = new Core.Graph.Presets.Encrypt.StandardEncrypt();
        var encryptGraph = sut.LoadPreset(nativeDefaultEncryptPreset, InstantiateNode, encryptParameters);

        using var inputStream = new MemoryStream();
        var decryptParameters = new Dictionary<string, object>
        {
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, password).SecurePassword },
            { "InputStream", inputStream }
        };
        var nativeDefaultDecryptPreset = new Core.Graph.Presets.Decrypt.StandardDecrypt(new DataParserSectionParser());

        var decryptGraph = sut.LoadPreset(nativeDefaultDecryptPreset, InstantiateNode, decryptParameters);

        // Act
        await encryptGraph.ExecuteAsync(CancellationToken.None);
        outputStream.Seek(0, SeekOrigin.Begin);
        await outputStream.CopyToAsync(inputStream);
        inputStream.Seek(0, SeekOrigin.Begin);
        await decryptGraph.ExecuteAsync(CancellationToken.None);

        // Assert
        var decryptedCipherText = System.Text.Encoding.UTF8.GetString((byte[])decryptGraph.OutputPins["DecryptedBytes"].ObjectValue);
        Assert.Equal(plainText, decryptedCipherText);
    }

    [Fact]
    public async Task GivenNativeDefaultEncryptPreset_AndParameters_WhenLoadPreset_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputStreamContainsData()
    {
        // Arrange
        using var outputStream = new MemoryStream();
        var sut = Instantiate();
        var parameters = new Dictionary<string, object>
        {
            { "SaltLength", 16 },
            { "IvLength", 16 },
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "OutputStream", outputStream },
            { "PlainText", "Hello World!" },
        };
        var nativeDefaultEncryptPreset = new Core.Graph.Presets.Encrypt.StandardEncrypt();

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
        using var inputStream = new MemoryStream(Convert.FromBase64String("BWnAbP+4WxKU1PgZjkb6l//xlo3PEqQOXjrwcVmjLMf11CuQwg/+CSmEIuBWzQ54"));
        var sut = Instantiate();
        var parameters = new Dictionary<string, object>
        {
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "InputStream", inputStream }
        };
        var nativeDefaultDecryptPreset = new Core.Graph.Presets.Decrypt.StandardDecrypt(new DataParserSectionParser());

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
        using var outputStream = new MemoryStream();
        var sut = Instantiate();
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
        using var inputStream = new MemoryStream(Convert.FromBase64String("BWnAbP+4WxKU1PgZjkb6l//xlo3PEqQOXjrwcVmjLMf11CuQwg/+CSmEIuBWzQ54"));
        var sut = Instantiate();
        var parameters = new Dictionary<string, object>
        {
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "InputStream", inputStream }
        };

        // Act
        var graph = sut.LoadNative(
            Enums.GraphContext.Decrypt,
            Enums.NativeGraphs.Default,
            InstantiateNode,
            parameters);

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

    private GraphFactory Instantiate()
    {
        var presets = new List<IGraphPreset>
        {
            new Core.Graph.Presets.Encrypt.StandardEncrypt(),
            new Core.Graph.Presets.Decrypt.StandardDecrypt(new DataParserSectionParser())
        };
        return new GraphFactory(presets);
    }
}
