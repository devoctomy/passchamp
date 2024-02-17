using devoctomy.Passchamp.Core.Extensions;
using devoctomy.Passchamp.Core.Graph.Services;
using devoctomy.Passchamp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Services;

public class GraphLoaderServiceTests
{
    public GraphLoaderServiceTests()
    {
        File.Delete("Output/test.dat");
    }

    [Theory]
    [InlineData("Data/complexgraph1.json")]
    public async Task GivenFileName_WhenLoadAsync_ThenGraphLoaded_AndGraphReturned_AndDetailsCorrect(string fileName)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var options = new PasschampCoreServicesOptions
        {
            CloudStorageProviderConfigLoaderServiceOptions = new Core.Cloud.CloudStorageProviderConfigLoaderServiceOptions()
        };
        serviceCollection.AddPasschampCoreServices(options);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var sut = new GraphLoaderService(
            new InputPinsJsonParserService(new TypeResolverService()),
            new OutputPinsJsonParserService(),
            new NodesJsonParserService(serviceProvider),
            null);

        // Act
        var result = await sut.LoadAsync(
            fileName,
            null,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Graph used for unit testing", result.Settings.Description);
        Assert.Equal("devoctomy", result.Settings.Author);
        Assert.Equal(6, result.InputPins.Count);
        Assert.Single(result.OutputPins);
        Assert.Equal(7, result.Nodes.Count);
        Assert.True(result.Nodes.ContainsKey("saltgenerator"));
        Assert.True(result.Nodes.ContainsKey("ivgenerator"));
        Assert.True(result.Nodes.ContainsKey("derive"));
        Assert.True(result.Nodes.ContainsKey("encode"));
        Assert.True(result.Nodes.ContainsKey("encrypt"));
        Assert.True(result.Nodes.ContainsKey("joiner"));
        Assert.True(result.Nodes.ContainsKey("writer"));
        Assert.True(result.Nodes.ContainsKey("writer"));
    }

    [Theory]
    [InlineData("Data/complexgraph1.json")]
    public async Task GivenFileName_WhenLoadAsync_henGraphLoadedAndExecuted_AndGraphReturned_AndExecutionOrderCorrect_AndOutputFileCreated(string fileName)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var options = new PasschampCoreServicesOptions
        {
            CloudStorageProviderConfigLoaderServiceOptions = new Core.Cloud.CloudStorageProviderConfigLoaderServiceOptions()
        };
        serviceCollection.AddPasschampCoreServices(options);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var sut = new GraphLoaderService(
            new InputPinsJsonParserService(new TypeResolverService()),
            new OutputPinsJsonParserService(),
            new NodesJsonParserService(serviceProvider),
            null);

        // Act
        var result = await sut.LoadAsync(
            fileName,
            null,
            CancellationToken.None);
        await result.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("saltgenerator,ivgenerator,derive,encode,encrypt,joiner,writer", string.Join(",", result.ExecutionOrder));
        Assert.True(File.Exists("Output/test.dat"));
    }

    [Theory]
    [InlineData("Data/complexgraph1.json")]
    public async Task GivenJsonDataStream_WhenLoadAsync_ThenGraphLoadedAndExecuted_AndGraphReturned_AndExecutionOrderCorrect_AndOutputFileCreated(string fileName)
    {
        // Arrange
        using var jsonDataStream = File.OpenRead(fileName);
        var serviceCollection = new ServiceCollection();
        var options = new PasschampCoreServicesOptions
        {
            CloudStorageProviderConfigLoaderServiceOptions = new Core.Cloud.CloudStorageProviderConfigLoaderServiceOptions()
        };
        serviceCollection.AddPasschampCoreServices(options);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var sut = new GraphLoaderService(
            new InputPinsJsonParserService(new TypeResolverService()),
            new OutputPinsJsonParserService(),
            new NodesJsonParserService(serviceProvider),
            null);

        // Act
        var result = await sut.LoadAsync(
            jsonDataStream,
            null,
            CancellationToken.None);
        await result.ExecuteAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("saltgenerator,ivgenerator,derive,encode,encrypt,joiner,writer", string.Join(",", result.ExecutionOrder));
        Assert.True(File.Exists("Output/test.dat"));
    }
}
