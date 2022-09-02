namespace devoctomy.Passchamp.Core.Cryptography.Random;

public interface IRandomNumericGenerator
{
    public double GenerateDouble();
    public int GenerateInt(
        int min,
        int max);
}
