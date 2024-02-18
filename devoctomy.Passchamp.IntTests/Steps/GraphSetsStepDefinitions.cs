using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Presets;
using devoctomy.Passchamp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
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
            var input = curRow.GetString("InputPath");
            var password = curRow.GetString("Password");

            Assert.NotNull(set);

            var inputData = await File.ReadAllTextAsync(input);
            using var outputStream = new MemoryStream();
            var parameters = new Dictionary<string, object>
            {
                { "SaltLength", 16 },
                { "IvLength", 16 },
                { "KeyLength", 32 },
                { "Passphrase", new NetworkCredential(null, password).SecurePassword },
                { "OutputStream", outputStream },
                { "PlainText", inputData },
            };

            var graph = graphFactory.LoadPreset(
                set.EncryptPreset,
                InstantiateNode,
                parameters);

            await graph.ExecuteAsync(CancellationToken.None);

            var encryptedBase64 = Convert.ToBase64String(outputStream.ToArray());

            // Create a copy of the encrypted data for reference
            await File.WriteAllBytesAsync($"Tests/Output/{setId}.{version}.enc", outputStream.ToArray());
            
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

            var decryptedBytes = (byte[])graph.OutputPins["DecryptedBytes"].ObjectValue;

            _scenarioContext.Add($"{setId}.{version}.DecryptedPlainText", System.Text.Encoding.UTF8.GetString(decryptedBytes));
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

            var inputData = await File.ReadAllBytesAsync($"Tests/Output/{setId}.{version}.enc");
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

            var decryptedBytes = (byte[])graph.OutputPins["DecryptedBytes"].ObjectValue;

            _scenarioContext.Add($"{setId}.{version}.Static.DecryptedPlainText", System.Text.Encoding.UTF8.GetString(decryptedBytes));
        }
    }

    [Then(@"decryption test results plain text matches input:")]
    public async Task ThenDecryptedPlainTextMatchesInput(Table table)
    {
        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));
            var input = curRow.GetString("InputPath");

            var expectedPlainText = await File.ReadAllTextAsync(input);
            var actualPlainText = _scenarioContext.Get<string>($"{setId}.{version}.DecryptedPlainText");

            Assert.Equal(
                expectedPlainText,
                actualPlainText);
        }
    }

    [Then(@"static decryption test results plain text matches input:")]
    public async Task ThenStaticDecryptedPlainTextMatchesInput(Table table)
    {
        foreach (var curRow in table.Rows)
        {
            var setId = curRow.GetString("SetId");
            var version = Version.Parse(curRow.GetString("Version"));
            var input = curRow.GetString("InputPath");

            var expectedPlainText = await File.ReadAllTextAsync(input);
            var actualPlainText = _scenarioContext.Get<string>($"{setId}.{version}.Static.DecryptedPlainText");

            Assert.Equal(
                expectedPlainText,
                actualPlainText);
        }
    }

    private INode InstantiateNode(Type type)
    {
        var serviceProvider = _scenarioContext.Get<IServiceProvider>("ServiceProvider");
        return (INode)serviceProvider.GetService(type);
    }
}