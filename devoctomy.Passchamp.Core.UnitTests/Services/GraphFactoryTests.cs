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
        var vault = new Core.Vault.Vault();
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
            { "Vault", vault },
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
        var resultVault = (Core.Vault.Vault)decryptGraph.OutputPins["Vault"].ObjectValue;
        Assert.Equal(vault.Id, resultVault.Id);
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
            { "Vault", new Core.Vault.Vault() },
        };
        var nativeDefaultEncryptPreset = new Core.Graph.Presets.Encrypt.StandardEncrypt();

        // Act
        var graph = sut.LoadPreset(nativeDefaultEncryptPreset, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        var pop = Convert.ToBase64String(outputStream.ToArray());
        Assert.True(outputStream.Length > 0);
    }

    [Fact]
    public async Task GivenNativeDefaultDecryptPreset_AndParameters_WhenLoadPreset_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputContainsCorrectData()
    {
        // Arrange
        using var inputStream = new MemoryStream(Convert.FromBase64String("8yj5tF0+6fwd1Bb86jif8ykF4/YVri/Hy/qTve1clOvQcvz8zxseYRnBoCTjPkAem7GcT6JzTDQ9fhAEvkM8fensKqFlIOnMWe0E/JowXdyAJRl+vcvyloeoimqmhkxEg4sBHIq5wHn2H2uIs7uuJbFuv8P2Vc00MrnDaFlvJ3VO8YnMXB6WU+NvM3NGWA8W6YcSaS21bw9xwV2kKegzNvvQxLAy7lIoT0aLdG7UPkwU5/Mmueo2UZr7LrZSiABaJTrsPjv8OyKU4pRne5+flVFWJyGz7J8Jap2Y7Y7uON0="));
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
        var vault = (Core.Vault.Vault)graph.OutputPins["Vault"].ObjectValue;
        Assert.Equal("cef6de30-ee52-48a5-a145-c212accceb13", vault.Id);
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
            { "Vault", new Core.Vault.Vault() },
        };

        // Act
        var graph = sut.LoadNative(Enums.GraphContext.Encrypt, Enums.NativeGraphs.Default, InstantiateNode, parameters);

        // Assert
        await graph.ExecuteAsync(CancellationToken.None);
        Assert.True(outputStream.Length > 0);
    }

    [Fact]
    public void GivenNoneContext_AndDefaultNativeGraph_WhenLoadNative_ThenArgumentExceptionThrown()
    {
        // Arrange
        var sut = Instantiate();

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            var graph = sut.LoadNative(
                Enums.GraphContext.None,
                Enums.NativeGraphs.Default,
                InstantiateNode,
                null);
        });
    }

    [Fact]
    public void GivenDecryptContext_AndReservedNativeGraph_WhenLoadNative_ThenNotSupportedExceptionThrown()
    {
        // Arrange
        var sut = Instantiate();

        // Act & Assert
        Assert.ThrowsAny<NotSupportedException>(() =>
        {
            var graph = sut.LoadNative(
                Enums.GraphContext.Decrypt,
                Enums.NativeGraphs.Reserved,
                InstantiateNode,
                null);
        });
    }

    [Fact]
    public void GivenDecryptContext_AndNoneNativeGraph_AndParameters_WhenLoadNative_ThenArgumentExceptionThrown()
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

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() =>
        {
            var graph = sut.LoadNative(
                Enums.GraphContext.Decrypt,
                Enums.NativeGraphs.None,
                InstantiateNode,
                parameters);
        });
    }

    [Fact]
    public async Task GivenDecryptContext_AndDefaultNativeGraph_AndParameters_WhenLoadNative_ThenGraphCreated_AndGraphExecutesSuccessfully_AndOutputContainsCorrectData()
    {
        // Arrange
        using var inputStream = new MemoryStream(Convert.FromBase64String("8yj5tF0+6fwd1Bb86jif8ykF4/YVri/Hy/qTve1clOvQcvz8zxseYRnBoCTjPkAem7GcT6JzTDQ9fhAEvkM8fensKqFlIOnMWe0E/JowXdyAJRl+vcvyloeoimqmhkxEg4sBHIq5wHn2H2uIs7uuJbFuv8P2Vc00MrnDaFlvJ3VO8YnMXB6WU+NvM3NGWA8W6YcSaS21bw9xwV2kKegzNvvQxLAy7lIoT0aLdG7UPkwU5/Mmueo2UZr7LrZSiABaJTrsPjv8OyKU4pRne5+flVFWJyGz7J8Jap2Y7Y7uON0="));
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
        var vault = (Core.Vault.Vault)graph.OutputPins["Vault"].ObjectValue;
        Assert.Equal("cef6de30-ee52-48a5-a145-c212accceb13", vault.Id);
    }

    [Fact]
    public async Task GivenStandardSet_AndParameters_WhenLoadPresetSet_ThenEncrypt_AndDecrypt_AndPlainTextRecovered()
    {
        // Arrange
        var vault = new Core.Vault.Vault();
        using var stream = new MemoryStream();
        var presets = new List<IGraphPreset>
        {
            new Core.Graph.Presets.Encrypt.StandardEncrypt(),
            new Core.Graph.Presets.Decrypt.StandardDecrypt(new DataParserSectionParser())
        };
        var sut = new GraphFactory(presets);
        var parameters = new Dictionary<string, object>
        {
            { "SaltLength", 16 },
            { "IvLength", 16 },
            { "KeyLength", 32 },
            { "Passphrase", new NetworkCredential(null, "password123").SecurePassword },
            { "InputStream", stream },
            { "OutputStream", stream },
            { "Vault", vault },
        };
        var presetSet = new Core.Graph.Presets.Sets.StandardSet(presets);

        // Act
        var (encrypt, decrypt) = sut.LoadPresetSet(
            presetSet,
            InstantiateNode,
            parameters);

        await encrypt.ExecuteAsync(CancellationToken.None);
        stream.Seek(0, SeekOrigin.Begin);
        await decrypt.ExecuteAsync(CancellationToken.None);
        var resultVault = (Core.Vault.Vault)decrypt.OutputPins["Vault"].ObjectValue;
        Assert.Equal(vault.Id, resultVault.Id);
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

    private static GraphFactory Instantiate()
    {
        var presets = new List<IGraphPreset>
        {
            new Core.Graph.Presets.Encrypt.StandardEncrypt(),
            new Core.Graph.Presets.Decrypt.StandardDecrypt(new DataParserSectionParser())
        };
        return new GraphFactory(presets);
    }
}
