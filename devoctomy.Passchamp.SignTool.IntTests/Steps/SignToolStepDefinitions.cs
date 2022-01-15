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
            var processStartInfo = new ProcessStartInfo(
                "devoctomy.Passchamp.SignTool.exe",
                _scenarioContext.Get<string>("arguments"));
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
        }

        [Then(@"private key file generated of (.*) bytes")]
        public void ThenPrivateKeyFileGenerated(int bytes)
        {
            var file = new FileInfo("privatekey.json");
            Assert.True(file.Exists);
            Assert.Equal(bytes, file.Length);
        }

        [Then(@"public key file generated of (.*) bytes")]
        public void ThenPublicKeyFileGenerated(int bytes)
        {
            var file = new FileInfo("publickey.json");
            Assert.True(file.Exists);
            Assert.Equal(bytes, file.Length);
        }
    }
}
