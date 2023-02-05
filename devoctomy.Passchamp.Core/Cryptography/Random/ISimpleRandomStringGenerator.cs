namespace devoctomy.Passchamp.Core.Cryptography.Random;

public interface ISimpleRandomStringGenerator
{
    char GetRandomCharFromChars(string chars);
    string GenerateRandomStringFromChars(
        string chars,
        int length);
}
