using AutoFixture;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using devoctomy.Passchamp.Core.Vault;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace devoctomy.Passchamp.IntTests.Steps;

[Binding]
public sealed class GraphSetsStepDefinitions(ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    [Given(@"All core services registered to service provider")]
    public void GivenAllServicesRegisteredToContainer()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddPasschampCoreServices(new PasschampCoreServicesOptions
        {
            CloudStorageProviderConfigLoaderServiceOptions = new()
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _scenarioContext.Add("ServiceProvider", serviceProvider);
    }

    [Given(@"a sample random vault")]
    public void GivenASampleVault()
    {
        var fixture = new Fixture();
        var vault = fixture.Create<Vault>();
        _scenarioContext.Add("NewRandomVault", vault);
    }

    [When(@"run encryption tests:")]
    public async Task WhenRunEncryptionTests(Table table)
    {
        var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
        var presetSets = serviceProvider.GetService<IEnumerable<IGraphPresetSet>>();
        var graphFactory = serviceProvider.GetService<IGraphFactory>();

        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));
            var set = presetSets.Single(x => x.Id == setId && x.Version.Equals(version));
            var password = curRow.GetString("Password");
            var vault = _scenarioContext.Get<Core.Vault.Vault>("NewRandomVault");

            Assert.NotNull(set);

            using var outputStream = new MemoryStream();
            var parameters = new Dictionary<string, object>
            {
                { "SaltLength", 16 },
                { "IvLength", 16 },
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, password).SecurePassword },
                { "OutputStream", outputStream },
                { "Vault", vault },
            };

            var graph = graphFactory.LoadPreset(
                set.EncryptPreset,
                InstantiateNode,
                parameters);

            await graph.ExecuteAsync(CancellationToken.None);

            var encryptedBase64 = Convert.ToBase64String(outputStream.ToArray());

            // Create a copy of the encrypted data for reference
            await File.WriteAllBytesAsync($"Tests/Output/{setId}.{version}.enc", outputStream.ToArray());
            await File.WriteAllTextAsync($"Tests/Output/{setId}.{version}.json", JsonConvert.SerializeObject(vault, Formatting.None));

            _scenarioContext.Add($"{setId}.{version}.EncryptedBase64", encryptedBase64);
        }
    }

    [When(@"run decryption tests:")]
    public async Task WhenRunDecryptionTests(Table table)
    {
        var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
        var presetSets = serviceProvider.GetService<IEnumerable<IGraphPresetSet>>();
        var graphFactory = serviceProvider.GetService<IGraphFactory>();

        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));
            var set = presetSets.Single(x => x.Id == setId && x.Version.Equals(version));
            var password = curRow.GetString("Password");

            var inputBase64 = _scenarioContext.Get<string>($"{setId}.{version}.EncryptedBase64");
            using var inputStream = new MemoryStream(Convert.FromBase64String(inputBase64));
            var parameters = new Dictionary<string, object>
            {
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, password).SecurePassword },
                { "InputStream", inputStream }
            };

            var graph = graphFactory.LoadPreset(
                set.DecryptPreset,
                InstantiateNode,
                parameters);

            await graph.ExecuteAsync(CancellationToken.None);

            var decryptedVault = (Core.Vault.Vault)graph.OutputPins["Vault"].ObjectValue;
            _scenarioContext.Add($"{setId}.{version}.DecryptedVault", decryptedVault);
        }
    }

    [When(@"run static decryption tests:")]
    public async Task WhenRunStaticDecryptionTests(Table table)
    {
        var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
        var presetSets = serviceProvider.GetService<IEnumerable<IGraphPresetSet>>();
        var graphFactory = serviceProvider.GetService<IGraphFactory>();

        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));
            var set = presetSets.Single(x => x.Id == setId && x.Version.Equals(version));
            var password = curRow.GetString("Password");

            var inputData = await File.ReadAllBytesAsync($"Tests/{setId}/{version}/vault.enc");
            using var inputStream = new MemoryStream(inputData);
            var parameters = new Dictionary<string, object>
            {
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, password).SecurePassword },
                { "InputStream", inputStream }
            };

            var graph = graphFactory.LoadPreset(
                set.DecryptPreset,
                InstantiateNode,
                parameters);

            await graph.ExecuteAsync(CancellationToken.None);

            var decryptedVault = (Core.Vault.Vault)graph.OutputPins["Vault"].ObjectValue;
            _scenarioContext.Add($"{setId}.{version}.Static.DecryptedVault", decryptedVault);
        }
    }

    [Then(@"decryption test results match input:")]
    public void ThenDecryptionTestResultsMatchInput(Table table)
    {
        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));

            var inputVault = _scenarioContext.Get<Core.Vault.Vault>($"NewRandomVault");
            var decryptedVault = _scenarioContext.Get<Core.Vault.Vault>($"{setId}.{version}.DecryptedVault");

            Assert.Equal(
                JsonConvert.SerializeObject(inputVault, Formatting.None),
                JsonConvert.SerializeObject(decryptedVault, Formatting.None));
        }
    }

    [Then(@"static decryption test results matches expected:")]
    public async Task ThenStaticDecryptedPlainTextMatchesInput(Table table)
    {
        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));

            var expectedJson = await File.ReadAllTextAsync($"Tests/{setId}/{version}/vault.json");
            expectedJson = JObject.Parse(expectedJson).ToString(Formatting.None);
            var decryptedVault = _scenarioContext.Get<Core.Vault.Vault>($"{setId}.{version}.Static.DecryptedVault");

            var decryptedJson = JsonConvert.SerializeObject(decryptedVault);

            Assert.Equal(
                expectedJson,
                decryptedJson);
        }
    }

    private INode InstantiateNode(Type type)
    {
        var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
        return (INode)serviceProvider.GetService(type);
    }
}