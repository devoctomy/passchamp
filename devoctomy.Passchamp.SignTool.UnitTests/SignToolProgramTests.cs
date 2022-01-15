using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.SignTool.UnitTests
{
    public class SignToolProgramTests
    {
        [Fact]
        public async Task GivenInvalidArguments_WhenRun_ThenErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var options = new ParseResults
            {
                Exception = new Exception("Hello World!")
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.IsAny<Type>(),
                It.IsAny<string>(),
                out options)).Returns(false);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenGenerateArguments_WhenRun_ThenGenerateArgumentsNotParsed_AndErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Mode = Mode.Generate
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var generateOptions = new ParseResults
            {
                Exception = new Exception("Hello World!")
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(GenerateOptions)),
                It.IsAny<string>(),
                out generateOptions)).Returns(false);

            mockGenerateService.Setup(x => x.Generate(
                It.IsAny<GenerateOptions>()))
                .ReturnsAsync(0);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenArguments_AndUnknownMode_WhenRun_ThenErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Mode = Mode.None
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenGenerateArguments_WhenRun_ThenGenerateArgumentsParsed_AndGenerateRun_AndResultsReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Mode = Mode.Generate
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var generateOptions = new ParseResults
            {
                Options = new GenerateOptions
                {
                    Mode = Mode.Generate
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(GenerateOptions)),
                It.IsAny<string>(),
                out generateOptions)).Returns(true);

            mockGenerateService.Setup(x => x.Generate(
                It.IsAny<GenerateOptions>()))
                .ReturnsAsync(0);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(0, result);
            mockGenerateService.Verify(x => x.Generate(
                It.IsAny<GenerateOptions>()), Times.Once);
        }

        [Fact]
        public async Task GivenSignArguments_WhenRun_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Mode = Mode.Sign
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            // Act
            await Assert.ThrowsAnyAsync<NotImplementedException>(async () =>
            {
                await sut.Run();
            });
        }

        [Fact]
        public async Task GivenVerifyArguments_WhenRun_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockGenerateService = new Mock<IGenerateService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                mockGenerateService.Object);

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Mode = Mode.Verify
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            // Act
            await Assert.ThrowsAnyAsync<NotImplementedException>(async () =>
            {
                await sut.Run();
            });
        }
    }
}
