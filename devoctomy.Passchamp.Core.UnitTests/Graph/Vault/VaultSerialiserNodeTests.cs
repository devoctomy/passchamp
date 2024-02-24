using devoctomy.Passchamp.Core.Graph.Vault;
using devoctomy.Passchamp.Core.Vault;
using System.Collections.Generic;
using System;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using devoctomy.Passchamp.Core.Graph;
using Moq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Vault;

public class VaultSerialiserNodeTests
{
    [Fact]
    public async Task GivenVault_WhenExecuteAsync_ThenVaultSerialisedCorrectly()
    {
        // Arrange
        var now = new DateTime(2024, 1, 1);
        var vault = new Core.Vault.Vault
        {
            Header = new VaultHeader
            {
                FormatVersion = "format version"
            },
            Id = "3CBD42BF-EF6A-4AC0-893C-F5806A9E6101",
            Name = "vault name",
            Description = "vault description",
            CreatedAt = now.Subtract(TimeSpan.FromDays(1)).ToString("dd-MM-yyyy HH:mm:ss"),
            LastUpdatedAt = now.Subtract(TimeSpan.FromSeconds(30)).ToString("dd-MM-yyyy HH:mm:ss"),
            Credentials = new List<Credential>
            {
                new Credential
                {
                    ID = "F9F5956A-4AD2-44C2-ADA9-E2F927C35B8D",
                    GlyphKey = "glyph key",
                    GlyphColour = "glyph colour",
                    Name = "credential name",
                    Description = "credential description",
                    Website = "http://www.website.com",
                    CreatedAt = now.Subtract(TimeSpan.FromSeconds(45)).ToString("dd-MM-yyyy HH:mm:ss"),
                    LastUpdatedAt = now.Subtract(TimeSpan.FromSeconds(30)).ToString("dd-MM-yyyy HH:mm:ss"),
                    PasswordLastModifiedAt = now.Subtract(TimeSpan.FromSeconds(40)).ToString("dd-MM-yyyy HH:mm:ss"),
                    UserName = "username",
                    Password = "password",
                    Tags = new List<string>
                    {
                        "apple",
                        "orange",
                        "pear"
                    },
                    Notes = "some notes about the credential",
                    AuditLogEntries = new List<AuditLogEntry>
                    {
                        new AuditLogEntry
                        {
                            DateTime = now.Subtract(TimeSpan.FromSeconds(45)).ToString("dd-MM-yyyy HH:mm:ss"),
                            TypeOfEntry = EntryType.AddCredential.ToString(),
                            Parameters = new Dictionary<string, string>
                            {
                                { "bob", "hoskins" },
                                { "hoskins", "bob" },
                            }
                        },
                        new AuditLogEntry
                        {
                            DateTime = now.Subtract(TimeSpan.FromSeconds(40)).ToString("dd-MM-yyyy HH:mm:ss"),
                            TypeOfEntry = EntryType.ModifyPassword.ToString(),
                            Parameters = new Dictionary<string, string>
                            {
                                { "bob", "hoskins" },
                                { "hoskins", "bob" },
                            }
                        }
                    }
                }
            },
            AuditLogEntries = new List<AuditLogEntry>
            {
                new AuditLogEntry
                {
                    DateTime = now.Subtract(TimeSpan.FromSeconds(45)).ToString("dd-MM-yyyy HH:mm:ss"),
                    TypeOfEntry = EntryType.CreatedVault.ToString(),
                    Parameters = new Dictionary<string, string>
                    {
                        { "bob", "hoskins" },
                        { "hoskins", "bob" },
                    }
                }
            }
        };
        var sut = new VaultSerialiserNode
        {
            Vault = (IDataPin<Core.Vault.Vault>)DataPinFactory.Instance.Create(
                "Vault",
                vault)
        };
        var mockGraph = new Mock<IGraph>();
        var mockNextNode = new Mock<INode>();
        mockGraph.Setup(x => x.GetNode<INode>(
            It.Is<string>(x => x == sut.NextKey)))
            .Returns(mockNextNode.Object);
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act
        await sut.ExecuteAsync(cancellationTokenSource.Token);

        // Assert
        var expected = await File.ReadAllTextAsync("Data/VaultSerialiserNodeExpected.json");
        expected = JObject.Parse(expected).ToString(Formatting.None);
        Assert.Equal(expected, sut.VaultJson.Value);
    }
}
