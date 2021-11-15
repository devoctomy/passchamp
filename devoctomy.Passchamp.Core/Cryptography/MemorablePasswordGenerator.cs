using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Cryptography
{
    public class MemorablePasswordGenerator : IMemorablePasswordGenerator
    {
        private readonly IEnumerable<IMemorablePasswordSectionGenerator> _memorablePasswordSectionGenerators;
        private readonly IWordListLoader _wordListLoader;

        public MemorablePasswordGenerator(
            IEnumerable<IMemorablePasswordSectionGenerator> memorablePasswordSectionGenerators,
            IWordListLoader wordListLoader)
        {
            _memorablePasswordSectionGenerators = memorablePasswordSectionGenerators;
            _wordListLoader = wordListLoader;
        }

        public async Task<string> GenerateAsync(
            string pattern,
            CancellationToken cancellationToken)
        {
            var context = new MemorablePasswordGeneratorContext
            {
                WordLists = await _wordListLoader.LoadAllAsync(cancellationToken),
            };
            var phrase = pattern;
            var tokenRegex = new Regex("\\{(.*?)\\}");
            var invalidTokens = new Dictionary<string, string>();
            var tokenMatches = tokenRegex.Matches(pattern);
            while (tokenMatches.Count > 0)
            {
                var currentGeneratedPart = string.Empty;
                var firstMatch = tokenMatches[0];
                string token = firstMatch.Value.Trim(new char[] { '{', '}' });
                string[] tokenParts = token.Split(':');

                if (tokenParts.Length == 2)
                {
                    var generator = _memorablePasswordSectionGenerators.SingleOrDefault(x => x.IsApplicable(tokenParts[0]));
                    if (generator == null)
                    {
                        throw new ArgumentException($"Invalid token '{tokenParts[0]}' found in pattern.");
                    }

                    currentGeneratedPart = generator.Generate(
                        context,
                        tokenParts[1]);

                    phrase = phrase.Remove(firstMatch.Index, firstMatch.Length);
                    phrase = phrase.Insert(firstMatch.Index, currentGeneratedPart);
                }

                tokenMatches = tokenRegex.Matches(phrase);
            }

            return phrase;
        }
    }
}
