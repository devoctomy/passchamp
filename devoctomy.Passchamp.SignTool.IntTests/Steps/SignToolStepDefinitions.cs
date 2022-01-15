using System.Diagnostics;
using System.IO;
using TechTalk.SpecFlow;

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

        [When(@"run")]
        public void WhenRun()
        {
            var processStartInfo = new ProcessStartInfo(
                "devoctomy.Passchamp.SignTool.exe",
                _scenarioContext.Get<string>("arguments"));
            var process = Process.Start(processStartInfo);
            process.WaitForExit();
        }

        [Then(@"private key file generated")]
        public void ThenPrivateKeyFileGenerated()
        {
            File.Exists("privatekey.json");
        }

        [Then(@"public key file generated")]
        public void ThenPublicKeyFileGenerated()
        {
            File.Exists("publickey.json");
        }
    }
}
