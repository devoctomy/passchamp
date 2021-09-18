using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Vault;
using devoctomy.Passchamp.Core.Vault;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Vault
{
    public class VaultParserNodeTests
    {
        [Fact]
        public async Task GivenVaultJson_WhenExecuteAsync_ThenVaultParsedCorrectly()
        {
            // Arrange
            var now = DateTime.Now;
            var vault = new Core.Vault.Vault
            {
                Header = new VaultHeader
                {
                    FormatVersion = "format version"
                },
                ID = Guid.NewGuid().ToString(),
                Name = "vault name",
                Description = "vault description",
                CreatedAt = now.Subtract(TimeSpan.FromDays(1)).ToString("dd-MM-yyyy HH:mm:ss"),
                LastUpdatedAt = now.Subtract(TimeSpan.FromSeconds(30)).ToString("dd-MM-yyyy HH:mm:ss"),
                Credentials = new List<Credential>
                {
                    new Credential
                    {
                        ID = Guid.NewGuid().ToString(),
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
            var vaultJson = JsonConvert.SerializeObject(
                vault,
                Formatting.Indented);
            var sut = new VaultParserNode
            {
                VaultJson = (IDataPin<string>)DataPinFactory.Instance.Create(
                    "VaultJson",
                    vaultJson),
                NextKey = "hello"
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
            Assert.NotNull(sut.Vault);
            var parsedVaultJson = JsonConvert.SerializeObject(
                sut.Vault.ObjectValue,
                Formatting.Indented);
            Assert.Equal(vaultJson, parsedVaultJson);
        }

    }
}
