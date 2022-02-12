using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using TechTalk.SpecFlow;
using Xunit;

namespace devoctomy.Passchamp.SignTool.IntTests.Steps
{
    [Binding]
    public class SignToolStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public SignToolStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"generate command and no options")]
        public void GivenGenerateCommandAndNoOptions()
        {
            _scenarioContext.Set("generate", "arguments");
        }

        [Given(@"generate command and key length of (.*)")]
        public void GivenGenerateCommandAndKeyLengthOf(int length)
        {
            _scenarioContext.Set($"generate -l={length}", "arguments");
        }

        [When(@"run")]
        public void WhenRun()
        {
            var arguments = _scenarioContext.Get<string>("arguments");
            var processStartInfo = new ProcessStartInfo(
                "devoctomy.Passchamp.SignTool.exe",
                arguments); 
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
            _scenarioContext.Set(process.ExitCode, "exitCode");         
        }

        [Then(@"private key file generated of (.*) bytes")]
        public static void ThenPrivateKeyFileGenerated(int bytes)
        {
            var file = new FileInfo("privatekey.json");
            Assert.True(file.Exists);
            Assert.Equal(bytes, file.Length);
        }

        [Then(@"public key file generated of (.*) bytes")]
        public static void ThenPublicKeyFileGenerated(int bytes)
        {
            var file = new FileInfo("publickey.json");
            Assert.True(file.Exists);
            Assert.Equal(bytes, file.Length);
        }

        [Given(@"sign command")]
        public void GivenSignCommand()
        {
            _scenarioContext.Set("sign", "arguments");
        }

        [Given(@"verify command")]
        public void GivenVerifyCommand()
        {
            _scenarioContext.Set("verify", "arguments");
        }

        [Given("private key file of \"(.*)\"")]
        public void GivenPrivateKeyFileOf(string privateKeyFile)
        {
            Assert.True(File.Exists(privateKeyFile));
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -p=\"{privateKeyFile}\"";
            _scenarioContext.Set(arguments, "arguments");
        }

        [Given("public key file of \"(.*)\"")]
        public void GivenPublicKeyFileOf(string publicKeyFile)
        {
            Assert.True(File.Exists(publicKeyFile));
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -p=\"{publicKeyFile}\"";
            _scenarioContext.Set(arguments, "arguments");
        }

        [Given("input file of \"(.*)\"")]
        public void GivenInputFileOf(string inputFile)
        {
            Assert.True(File.Exists(inputFile));
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -i=\"{inputFile}\"";
            _scenarioContext.Set(arguments, "arguments");
            _scenarioContext.Set(inputFile, "input");
        }

        [Given("output filename of \"(.*)\"")]
        public void GivenOutputFilenameOf(string outputFile)
        {
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -o=\"{outputFile}\"";
            _scenarioContext.Set(arguments, "arguments");
            _scenarioContext.Set(outputFile, "outputfile");
        }

        [Given("modify input file \"(.*)\" and save as \"(.*)\"")]
        public void GivenModifyInputFileAndSaveAs(
            string inputFile,
            string outputFile)
        {
            var rawJson = File.ReadAllText(inputFile);
            var json = JObject.Parse(rawJson);
            json["SomeField"] = "Bob Hoskins";
            File.WriteAllText(outputFile, json.ToString(Newtonsoft.Json.Formatting.Indented));
            _scenarioContext.Set(outputFile, "input");
        }


        [Then(@"signature present in json")]
        public void ThenSignaturePresentInJson()
        {
            var output = new FileInfo(_scenarioContext.Get<string>("outputfile"));
            Assert.True(output.Exists, "Signed output file was not generated.");
            var outputRaw = File.ReadAllText(output.FullName);
            var outputJson = JObject.Parse(outputRaw);
            Assert.True(outputJson.ContainsKey("Signature"));
        }

        [Then(@"verify successful")]
        public void ThenVerifySuccessful()
        {
            var exitCode = _scenarioContext.Get<int>("exitCode");
            Assert.Equal(0, exitCode);
        }

        [Then(@"verify failed")]
        public void ThenVerifyFailed()
        {
            var exitCode = _scenarioContext.Get<int>("exitCode");
            Assert.True(exitCode != 0);
        }
    }
}
