using System;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Cryptography.Random;

public class MemorablePasswordWordListSectionGenerator : IMemorablePasswordSectionGenerator
{
    private readonly IRandomNumericGenerator _randomNumericGenerator;

    public MemorablePasswordWordListSectionGenerator(IRandomNumericGenerator randomNumericGenerator)
    {
        _randomNumericGenerator = randomNumericGenerator;
    }

    public bool IsApplicable(string token)
    {
        return token.Equals("wordlist", StringComparison.InvariantCultureIgnoreCase);
    }

    public string Generate(
        MemorablePasswordGeneratorContext context,
        string arguments)
    {
        var argumentParts = arguments.Split('_');
        if (!context.WordLists.ContainsKey(argumentParts[0]))
        {
            throw new ArgumentException($"List with the name {argumentParts[0]} was not found.");
        }

        var randomWord = GetRandomWordFromList(context.WordLists[argumentParts[0]]);
        var casing = argumentParts[1].ToLowerInvariant();
        if (casing == "rc")
        {
            var casingOptions = new string[] { "lc", "uc", "ic" };
            casing = casingOptions[_randomNumericGenerator.GenerateInt(0, casingOptions.Length)];
        }

        switch (casing)
        {
            case "lc":
                {
                    randomWord = randomWord.ToLowerInvariant();
                    break;
                }

            case "uc":
                {
                    randomWord = randomWord.ToUpperInvariant();
                    break;
                }

            case "ic":
                {
                    randomWord = randomWord[0].ToString().ToUpperInvariant() + randomWord[1..];
                    break;
                }

            default:
                {
                    throw new ArgumentException($"Unknown casing mode '{casing}'.");
                }
        }

        return randomWord;
    }

    private string GetRandomWordFromList(List<string> wordList)
    {
        var randomIndex = _randomNumericGenerator.GenerateInt(0, wordList.Count);
        return wordList[randomIndex];
    }
}
