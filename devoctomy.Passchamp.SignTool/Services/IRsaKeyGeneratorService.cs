namespace devoctomy.Passchamp.SignTool.Services;

public interface IRsaKeyGeneratorService
{
    RsaKeyPair Generate(int keySize);
}
