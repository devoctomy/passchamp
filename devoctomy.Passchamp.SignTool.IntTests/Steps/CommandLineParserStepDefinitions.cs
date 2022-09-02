using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace devoctomy.Passchamp.SignTool.IntTests.Steps;

[Binding]
public sealed class CommandLineParserStepDefinitions
{

    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private CommandLineParserService _sut;

    public CommandLineParserStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the arguments (.*)")]
    public void GivenTheArguments(string argumentsString)
    {
        _scenarioContext.Set(argumentsString, "ArgumentsString");
    }

    [Given(@"options is of type (.*)")]
    public void GivenOptionsIsOfType(string optionsTypeName)
    {
        var optionsType = Type.GetType(optionsTypeName);
        Assert.NotNull(optionsType);
        _scenarioContext.Set(optionsType, "OptionsType");
    }

    [Given(@"command line parser service is a default instance")]
    public void GivenCommandLineParserServiceIsADefaultInstance()
    {
        _sut = CommandLineParserService.CreateDefaultInstance();
    }


    [When(@"TryParseArgumentsAsOptions")]
    public void WhenTryParseArgumentsAsOptions()
    {
        var optionsType = _scenarioContext["OptionsType"] as Type;
        var argumentsString = _scenarioContext.Get<string>("ArgumentsString");
        var result = _sut.TryParseArgumentsAsOptions(
            optionsType,
            argumentsString,
            out var parseResults);
        _scenarioContext.Set(result, "TryParseArgumentsAsOptionsResult");
        _scenarioContext.Set(parseResults, "ParseResults");
    }

    [Then(@"parsing was successful")]
    public void ThenParsingWasSuccessful()
    {
        var result = _scenarioContext.Get<bool>("TryParseArgumentsAsOptionsResult");
        Assert.True(result);
    }

    [Then(@"the options should match that of (.*)")]
    public async Task ThenTheOptionsShouldMatch(string expectedOptionsFile)
    {
        var optionsType = _scenarioContext["OptionsType"] as Type;
        var expectedOptionsJson = await File.ReadAllTextAsync(expectedOptionsFile).ConfigureAwait(false);
        var expectedOptions = JsonConvert.DeserializeObject(expectedOptionsJson, optionsType);
        var parseResults = _scenarioContext.Get<ParseResults>("ParseResults");
        var mismatchedProperties = GetNotEqualsProperties(
            parseResults.Options,
            expectedOptions);
        var fieldNames = string.Join(',', mismatchedProperties.Select(x => x.Name));
        Assert.False(mismatchedProperties.Any(), $"Found mismatched properties '{fieldNames}'.");
    }

    private static IEnumerable<PropertyInfo> GetNotEqualsProperties(
        object a,
        object b)
    {
        Type objectType = a.GetType();
        var properies = objectType.GetProperties();
        foreach (var property in properies)
        {
            var aVal = property.GetValue(a, null);
            var bVal = property.GetValue(b, null);
            if ((aVal != null && bVal != null) && !aVal.Equals(bVal))
            {
                yield return property;
            }
        }
    }
}