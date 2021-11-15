namespace devoctomy.Passchamp.Core.Cryptography
{
    public interface IMemorablePasswordSectionGenerator
    {
        bool IsApplicable(string token);
        string Generate(
            MemorablePasswordGeneratorContext context,
            string arguments);
    }
}
