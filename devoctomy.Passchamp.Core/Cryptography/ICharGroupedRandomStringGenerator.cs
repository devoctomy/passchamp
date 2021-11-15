using System;

namespace devoctomy.Passchamp.Core.Cryptography
{
    public interface ICharGroupedRandomStringGenerator
    {
        [Flags]
        public enum CharSelection
        {
            None = 0,
            Lowercase = 1,
            Uppercase = 2,
            Digits = 4,
            Minus = 8,
            Underline = 16,
            Space = 32,
            Brackets = 64,
            Other = 128,
            All = 255
        }

        string GenerateString(
            CharSelection charSelection,
            int length,
            bool atLeastOneOfEachGroup);
    }
}
