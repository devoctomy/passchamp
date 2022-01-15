using System;
using System.Linq;

namespace devoctomy.Passchamp.Core.Cryptography.Random
{
    public class CharGroupedRandomStringGenerator : ICharGroupedRandomStringGenerator
    {
        private readonly ISimpleRandomStringGenerator _simpleRandomStringGenerator;
        private readonly IRandomNumericGenerator _randomNumericGenerator;
        private readonly IStringChecker _stringChecker;

        public CharGroupedRandomStringGenerator(
            ISimpleRandomStringGenerator simpleRandomStringGenerator,
            IRandomNumericGenerator randomNumericGenerator,
            IStringChecker stringChecker)
        {
            _simpleRandomStringGenerator = simpleRandomStringGenerator;
            _randomNumericGenerator = randomNumericGenerator;
            _stringChecker = stringChecker;
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
            foreach(var curCharSelection in Enum.GetValues<ICharGroupedRandomStringGenerator.CharSelection>())
            {
                if(curCharSelection == ICharGroupedRandomStringGenerator.CharSelection.All)
                {
                    continue;
                }

                groupCount += (charSelection.HasFlag(curCharSelection)) ? 1 : 0;
            }

            if (atLeastOneOfEachGroup && length < groupCount)
            {
                throw new ArgumentException("Length must be greater than or equal to number of char groups in selection.", "length");
            }

            var fixing = true;
            while (fixing)
            {
                fixing = false;
                foreach (var curGroup in groups.Where(x => !_stringChecker.ContainsAtLeastOneOf(random, x.Chars)))
                {
                    fixing = true;
                    var single = _simpleRandomStringGenerator.GenerateRandomStringFromChars(curGroup.Chars, 1);
                    var index = _randomNumericGenerator.GenerateInt(1, random.Length) - 1;
                    random = random.Remove(index, 1);
                    random = random.Insert(index, single);
                }
            }

            return (random);
        }
    }
}
