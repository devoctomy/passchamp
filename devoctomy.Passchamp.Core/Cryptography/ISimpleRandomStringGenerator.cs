namespace devoctomy.Passchamp.Core.Cryptography
{
    public interface ISimpleRandomStringGenerator
    {
        char GetRandomCharFromChars(string chars);
        string GenerateRandomStringFromChars(
            string chars,
            int length);
    }
}
