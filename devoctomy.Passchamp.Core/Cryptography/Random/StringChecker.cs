namespace devoctomy.Passchamp.Core.Cryptography.Random;

public class StringChecker : IStringChecker
{
    public bool ContainsAtLeastOneOf(
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
