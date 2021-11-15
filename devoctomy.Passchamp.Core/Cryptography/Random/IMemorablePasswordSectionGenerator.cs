namespace devoctomy.Passchamp.Core.Cryptography.Random
{
    public interface IMemorablePasswordSectionGenerator
    {
        bool IsApplicable(string token);
        string Generate(
            MemorablePasswordGeneratorContext context,
            string arguments);
    }
}
