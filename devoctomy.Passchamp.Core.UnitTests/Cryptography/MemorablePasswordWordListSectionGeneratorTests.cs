using devoctomy.Passchamp.Core.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class MemorablePasswordWordListSectionGeneratorTests
    {
        [Fact]
        public void GivenApplicableToken_WhenIsApplicable_ThenTrueReturned()
        {
            // Arrange
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.IsApplicable("wordlist");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenNonApplicableToken_WhenIsApplicable_ThenFalseReturned()
        {
            // Arrange
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.IsApplicable("int");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenContext_AndLowerCaseArguments_WhenGenerate_ThenCorrectStringReturned()
        {
            // Arrange
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = new Dictionary<string, List<string>>
                {
                    { "fruits", new List<string> { "APPLE", "ORANGE", "BANANA", "PEAR" } }
                }
            };
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.Generate(context, "fruits_lc");

            // Assert
            Assert.Contains(result, context.WordLists["fruits"].Select(x => x.ToLower()));
        }

        [Fact]
        public void GivenContext_AndUpperCaseArguments_WhenGenerate_ThenCorrectStringReturned()
        {
            // Arrange
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = new Dictionary<string, List<string>>
                {
                    { "fruits", new List<string> { "apple", "orange", "banana", "pear" } }
                }
            };
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.Generate(context, "fruits_uc");

            // Assert
            Assert.Contains(result, context.WordLists["fruits"].Select(x => x.ToUpper()));
        }

        [Fact]
        public void GivenContext_AndInitialCapsArguments_WhenGenerate_ThenCorrectStringReturned()
        {
            // Arrange
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = new Dictionary<string, List<string>>
                {
                    { "fruits", new List<string> { "apple", "orange", "banana", "pear" } }
                }
            };
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.Generate(context, "fruits_ic");

            // Assert
            Assert.Contains(result, context.WordLists["fruits"].Select(x => x[0].ToString().ToUpper() + x.Substring(1)));
        }

        [Fact]
        public void GivenContext_AndRandomCaseArguments_WhenGenerate_ThenCorrectStringReturned()
        {
            // Arrange
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = new Dictionary<string, List<string>>
                {
                    { "fruits", new List<string> { "aPPLE", "oRANGE", "bANANA", "pEAR" } }
                }
            };
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act
            var result = sut.Generate(context, "fruits_rc");

            // Assert
            // Can we add another assertion in here?
            var allLowerCase = context.WordLists["fruits"].Select(x => x.ToLower()).ToList();
            var allUpperCase = context.WordLists["fruits"].Select(x => x.ToLower()).ToList();
            var allInitialCaps = context.WordLists["fruits"].Select(x => x[0].ToString().ToUpper() + x.Substring(1)).ToList();
            Assert.True(
                allLowerCase.Contains(result) ||
                allUpperCase.Contains(result) ||
                allInitialCaps.Contains(result));
        }

        [Fact]
        public void GivenContext_AndInvalidCaseArguments_WhenGenerate_ThenInvalidArgumentExceptionThrown()
        {
            // Arrange
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = new Dictionary<string, List<string>>
                {
                    { "fruits", new List<string> { "apple", "orange", "banana", "pear" } }
                }
            };
            var sut = new MemorablePasswordWordListSectionGenerator(new RandomNumericGenerator());

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                sut.Generate(context, "fruits_pc");
            });
        }
    }
}
