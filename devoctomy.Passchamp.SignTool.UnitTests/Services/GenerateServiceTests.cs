using devoctomy.Passchamp.SignTool.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests.Services
{
    public class GenerateServiceTests
    {
        public GenerateServiceTests()
        {
            File.Delete("privatekey.json");
            File.Delete("publickey.json");
        }

        [Fact]
        public async Task GivenGenerateOptions_WhenGenerate_ThenKeysGenerated()
        {
            // Arrange
            var options = new GenerateOptions
            {
                KeyLength = 1024,
                Verbose = true,     // Need to test messages were output
            };
            var sut = new GenerateService();

            // Act
            var result = await sut.Generate(options);

            // Assert
            File.Exists("privatekey.json");
            File.Exists("publickey.json");

            // Cleanup
            File.Delete("privatekey.json");
            File.Delete("publickey.json");
        }
    }
}
