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

        [Given("private key file of \"(.*)\"")]
        public void GivenPrivateKeyFileOf(string privateKeyFile)
        {
            Assert.True(File.Exists(privateKeyFile));
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -p=\"{privateKeyFile}\"";
            _scenarioContext.Set(arguments, "arguments");
        }

        [Given("input file of \"(.*)\"")]
        public void GivenJsonFileOf(string inputFile)
        {
            Assert.True(File.Exists(inputFile));
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -i=\"{inputFile}\"";
            _scenarioContext.Set(arguments, "arguments");
        }

        [Given(@"random output filename")]
        public void GivenRandomOutputFilename()
        {
            var outputfile = $"{System.Guid.NewGuid().ToString()}.json";
            var arguments = _scenarioContext.Get<string>("arguments");
            arguments += $" -o=\"{outputfile}\"";
            _scenarioContext.Set(arguments, "arguments");
            _scenarioContext.Set(outputfile, "outputfile");
        }

        [Then(@"signature present in json")]
        public void ThenSignaturePresentInJson()
        {
            var output = new FileInfo(_scenarioContext.Get<string>("outputfile"));
            Assert.True(output.Exists, "Signed output file was not generated.");
            var outputRaw = File.ReadAllText(output.FullName);
            var outputJson = JObject.Parse(outputRaw);
            Assert.True(outputJson.ContainsKey("Signature"));
            output.Delete();
        }
    }
}
