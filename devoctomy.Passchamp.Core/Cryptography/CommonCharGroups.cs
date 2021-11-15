using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace devoctomy.Passchamp.Core.Cryptography
{
    [ExcludeFromCodeCoverage]
    public static class CommonCharGroups
    {
        public static Dictionary<string, CharGroup> CharGroups = new Dictionary<string, CharGroup>
        {
            { "Lowercase", new CharGroup { Name = "Lowercase", Chars = "abcdefghijklmnopqrstuvwxyz" } },
            { "Uppercase", new CharGroup { Name = "Uppercase", Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" } },
            { "Digits", new CharGroup { Name = "Digits", Chars = "0123456789" } },
            { "Minus", new CharGroup { Name = "Minus", Chars = "-" } },
            { "Underline", new CharGroup { Name = "Underline", Chars = "_" } },
            { "Space", new CharGroup { Name = "Space", Chars = " " } },
            { "Brackets", new CharGroup { Name = "Brackets", Chars = "[]{}()<>" } },
            { "Other", new CharGroup { Name = "Other", Chars = "!£$%&+@#,.\\/" } },
        };
    }
}
