using System;

namespace devoctomy.Passchamp.Core.Cryptography.Random;

public class MemorablePasswordIntSectionGenerator : IMemorablePasswordSectionGenerator
{
    private readonly IRandomNumericGenerator _randomNumericGenerator;

    public MemorablePasswordIntSectionGenerator(IRandomNumericGenerator randomNumericGenerator)
    {
        _randomNumericGenerator = randomNumericGenerator;
    }

    public bool IsApplicable(string token)
    {
        return token.Equals("int", StringComparison.InvariantCultureIgnoreCase);
    }

    public string Generate(
        MemorablePasswordGeneratorContext context,
        string arguments)
    {
        string[] rangeParts = arguments.Split('_');
        int min = int.Parse(rangeParts[0]);
        int max = int.Parse(rangeParts[1]);
        if (max > min)
        {
            int randomInt = _randomNumericGenerator.GenerateInt(min, max);
            return randomInt.ToString();
        }

        throw new ArgumentException($"Max must be greater than min range value.");
    }
}
