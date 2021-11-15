using System;
using System.Linq;

namespace devoctomy.Passchamp.Core.Cryptography
{
    public class CharGroupedRandomStringGenerator : ICharGroupedRandomStringGenerator
    {
        private readonly ISimpleRandomStringGenerator _simpleRandomStringGenerator;
        private readonly IRandomNumericGenerator _randomNumericGenerator;

        public CharGroupedRandomStringGenerator(
            ISimpleRandomStringGenerator simpleRandomStringGenerator,
            IRandomNumericGenerator randomNumericGenerator)
        {
            _simpleRandomStringGenerator = simpleRandomStringGenerator;
            _randomNumericGenerator = randomNumericGenerator;
        }

        public string GenerateString(
            ICharGroupedRandomStringGenerator.CharSelection charSelection,
            int length,
            bool atLeastOneOfEachGroup)
        {
            var selection = charSelection.ToString().Split(',');
            var groups = charSelection == ICharGroupedRandomStringGenerator.CharSelection.All ? CommonCharGroups.CharGroups.Values.Select(x => x).ToList() : selection.Select(x => CommonCharGroups.CharGroups[x.ToString().Trim()]).ToList();
            var allChars = groups.Select(x => x.Chars).Aggregate((a, b) => a + b);
            var random = _simpleRandomStringGenerator.GenerateRandomStringFromChars(allChars, length);

            var groupCount = 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Lowercase)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Uppercase)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Digits)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Minus)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Underline)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Space)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Brackets)) ? 1 : 0;
            groupCount += (charSelection.HasFlag(ICharGroupedRandomStringGenerator.CharSelection.Other)) ? 1 : 0;

            if (atLeastOneOfEachGroup && length < groupCount)
            {
                throw new ArgumentException("Length must be greater than or equal to number of char groups in selection.", "length");
            }

            var fixing = true;
            while (fixing)
            {
                fixing = false;
                foreach (var curGroup in groups)
                {
                    if (!StringContainsOneOf(random, curGroup.Chars))
                    {
                        fixing = true;
                        var single = _simpleRandomStringGenerator.GenerateRandomStringFromChars(curGroup.Chars, 1);
                        var index = _randomNumericGenerator.GenerateInt(1, random.Length) - 1;
                        random = random.Remove(index, 1);
                        random = random.Insert(index, single);
                    }
                }
            }

            return (random);
        }

        private static bool StringContainsOneOf(
            string value,
            string chars)
        {
            foreach (char curChar in chars)
            {
                if (value.Contains(curChar.ToString()))
                {
                    return (true);
                }
            }

            return (false);
        }
    }
}
