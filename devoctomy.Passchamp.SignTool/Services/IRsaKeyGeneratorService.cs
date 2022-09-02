﻿using devoctomy.Passchamp.SignTool.Services.Enums;

namespace devoctomy.Passchamp.SignTool.Services;

public interface IRsaKeyGeneratorService
{
    void Generate(
        int keySize,
        out string privateKey,
        out string publicKey);
}
