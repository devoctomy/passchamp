using devoctomy.Passchamp.SignTool.Services.Enums;

namespace devoctomy.Passchamp.SignTool.Services;

public interface IRsaKeyGeneratorService
{
    RsaKeyPair Generate(int keySize);
}
