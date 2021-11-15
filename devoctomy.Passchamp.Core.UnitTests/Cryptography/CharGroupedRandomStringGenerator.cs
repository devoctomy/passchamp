using devoctomy.Passchamp.Core.Cryptography;
using System;
using System.Linq;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class CharGroupedRandomStringGeneratorTests
    {
        [Theory]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Uppercase, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Lowercase, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Digits, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Minus, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Brackets, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Other, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Space, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Underline, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.All, 10)]
        public void GivenCharGroups_AndLength_WhenGenerateString_ThenRandomStringReturnedOfCorrectLength(
            ICharGroupedRandomStringGenerator.CharSelection charSelection,
            int length)
        {
            // Arrange
            var randomNumericGenerator = new RandomNumericGenerator();
            var sut = new CharGroupedRandomStringGenerator(
                new SimpleRandomStringGenerator(randomNumericGenerator),
                randomNumericGenerator);
            var selection = charSelection.ToString().Split(',');
            var groups = charSelection == ICharGroupedRandomStringGenerator.CharSelection.All ? CommonCharGroups.CharGroups.Values.Select(x => x).ToList() : selection.Select(x => CommonCharGroups.CharGroups[x.ToString()]).ToList();
            var allChars = groups.Select(x => x.Chars).Aggregate((a, b) => a + b);

            // Act
            var result = sut.GenerateString(charSelection, length, false);

            // Assert
            Assert.Equal(length, result.Length);
            foreach(var curChar in result)
            {
                Assert.Contains(curChar, allChars);
            }
        }

        [Theory]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Uppercase, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.Uppercase | ICharGroupedRandomStringGenerator.CharSelection.Lowercase, 10)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.All, 100)]
        public void GivenCharGroups_AndLength_AndAtLeastOneOfEachGroup_WhenGenerateString_ThenRandomStringReturnedOfCorrectLength_AndAtLeastOneOfEachGroupPresent(
            ICharGroupedRandomStringGenerator.CharSelection charSelection,
            int length)
        {
            // Arrange
            var randomNumericGenerator = new RandomNumericGenerator();
            var sut = new CharGroupedRandomStringGenerator(
                new SimpleRandomStringGenerator(randomNumericGenerator),
                randomNumericGenerator);
            var selection = charSelection.ToString().Split(',');
            var groups = charSelection == ICharGroupedRandomStringGenerator.CharSelection.All ? CommonCharGroups.CharGroups.Values.Select(x => x).ToList() : selection.Select(x => CommonCharGroups.CharGroups[x.ToString().Trim()]).ToList();
            var allChars = groups.Select(x => x.Chars).Aggregate((a, b) => a + b);

            // Act
            var result = sut.GenerateString(charSelection, length, true);

            // Assert
            Assert.Equal(length, result.Length);
            foreach (var curChar in result)
            {
                Assert.Contains(curChar, allChars);
            }
            foreach (var curGroup in groups)
            {
                Assert.Contains(curGroup.Chars, x => result.Contains(x));
            }
        }

        [Theory]
        [InlineData(
            ICharGroupedRandomStringGenerator.CharSelection.Uppercase |
            ICharGroupedRandomStringGenerator.CharSelection.Lowercase, 1)]
        [InlineData(
            ICharGroupedRandomStringGenerator.CharSelection.Uppercase |
            ICharGroupedRandomStringGenerator.CharSelection.Lowercase |
            ICharGroupedRandomStringGenerator.CharSelection.Digits, 2)]
        [InlineData(ICharGroupedRandomStringGenerator.CharSelection.All, 7)]
        public void GivenCharGroups_AndShortLength_AndAtLeastOneOfEachGroup_WhenGenerateString_ThenArgumentExceptionThrown(
            ICharGroupedRandomStringGenerator.CharSelection charSelection,
            int length)
        {
            // Arrange
            var randomNumericGenerator = new RandomNumericGenerator();
            var sut = new CharGroupedRandomStringGenerator(
                new SimpleRandomStringGenerator(randomNumericGenerator),
                randomNumericGenerator);
            var selection = charSelection.ToString().Split(',');
            var groups = charSelection == ICharGroupedRandomStringGenerator.CharSelection.All ? CommonCharGroups.CharGroups.Values.Select(x => x).ToList() : selection.Select(x => CommonCharGroups.CharGroups[x.ToString().Trim()]).ToList();
            var allChars = groups.Select(x => x.Chars).Aggregate((a, b) => a + b);

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                sut.GenerateString(charSelection, length, true);
            });
        }
    }
}
