using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Cryptography.Random;

public class MemorablePasswordGeneratorContext
{
    public Dictionary<string, List<string>> WordLists { get; set; }
}
