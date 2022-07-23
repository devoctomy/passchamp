using devoctomy.Passchamp.SignTool.Services;
using devoctomy.Passchamp.SignTool.Services.CommandLineParser;
using devoctomy.Passchamp.SignTool.Services.Enums;
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
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

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
        public async Task GivenArguments_AndUnknownCommand_WhenRun_ThenErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.None
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
        public async Task GivenGenerateArguments_WhenRun_ThenGenerateArgumentsNotParsed_AndErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Generate
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

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenSignArguments_WhenRun_ThenSignArgumentsNotParsed_AndErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Sign
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var signOptions = new ParseResults
            {
                Exception = new Exception("Hello World!")
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(SignOptions)),
                It.IsAny<string>(),
                out signOptions)).Returns(false);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task GivenVerifyArguments_WhenRun_ThenVerifyArgumentsNotParsed_AndErrorCodeReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Verify
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var verifyOptions = new ParseResults
            {
                Exception = new Exception("Hello World!")
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(VerifyOptions)),
                It.IsAny<string>(),
                out verifyOptions)).Returns(false);

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
                mockGenerateService.Object,
                null,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Generate
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
                    Command = Command.Generate
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
        public async Task GivenSignArguments_WhenRun_ThenSignRun_AndResultReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockSignerService = new Mock<IRsaJsonSignerService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                mockSignerService.Object,
                null,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Sign
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var signOptions = new ParseResults
            {
                Options = new SignOptions
                {
                    Command = Command.Sign,
                    Input = Guid.NewGuid().ToString(),
                    KeyFile = Guid.NewGuid().ToString(),
                    Output = Guid.NewGuid().ToString()
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(SignOptions)),
                It.IsAny<string>(),
                out signOptions)).Returns(true);

            mockSignerService.Setup(x => x.Sign(
                It.IsAny<SignOptions>()))
                .ReturnsAsync(0);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(0, result);
            mockSignerService.Verify(x => x.Sign(
                It.Is<SignOptions>(y => y == signOptions.OptionsAs<SignOptions>())), Times.Once);
        }

        [Fact]
        public async Task GivenVerifyArguments_WhenRun_ThenVerifyRun_AndResultReturned()
        {
            // Arrange
            var mockCommandLineArgumentService = new Mock<ICommandLineArgumentService>();
            var mockCommandLineParserService = new Mock<ICommandLineParserService>();
            var mockVerifierService = new Mock<IRsaJsonVerifierService>();
            var sut = new SignToolProgram(
                mockCommandLineArgumentService.Object,
                mockCommandLineParserService.Object,
                null,
                null,
                mockVerifierService.Object,
                Mock.Of<IHelpMessageFormatter>());

            var preOptions = new ParseResults
            {
                Options = new PreOptions
                {
                    Command = Command.Verify
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(PreOptions)),
                It.IsAny<string>(),
                out preOptions)).Returns(true);

            var verifyOptions = new ParseResults
            {
                Options = new VerifyOptions
                {
                    Command = Command.Verify,
                    Input = Guid.NewGuid().ToString(),
                    KeyFile = Guid.NewGuid().ToString()
                }
            };
            mockCommandLineParserService.Setup(x => x.TryParseArgumentsAsOptions(
                It.Is<Type>(y => y == typeof(VerifyOptions)),
                It.IsAny<string>(),
                out verifyOptions)).Returns(true);

            mockVerifierService.Setup(x => x.Verify(
                It.IsAny<VerifyOptions>()))
                .ReturnsAsync(0);

            // Act
            var result = await sut.Run();

            // Assert
            Assert.Equal(0, result);
            mockVerifierService.Verify(x => x.Verify(
                It.Is<VerifyOptions>(y => y == verifyOptions.OptionsAs<VerifyOptions>())), Times.Once);
        }
    }
}
