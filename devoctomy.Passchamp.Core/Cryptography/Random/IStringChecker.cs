namespace devoctomy.Passchamp.Core.Cryptography.Random;

public interface IStringChecker
{
    bool ContainsAtLeastOneOf(
        string value,
        string chars);
}
