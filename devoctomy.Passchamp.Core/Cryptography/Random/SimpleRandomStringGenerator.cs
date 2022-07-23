using System.Text;

namespace devoctomy.Passchamp.Core.Cryptography.Random
{
    public class SimpleRandomStringGenerator : ISimpleRandomStringGenerator
    {
        private readonly IRandomNumericGenerator _randomNumericGenerator;

        public SimpleRandomStringGenerator(IRandomNumericGenerator randomNumericGenerator)
        {
            _randomNumericGenerator = randomNumericGenerator;
        }

        public char GetRandomCharFromChars(string chars)
        {
            var index = _randomNumericGenerator.GenerateInt(0, chars.Length);
            return chars[index];
        }

        public string GenerateRandomStringFromChars(
            string chars,
            int length)
        {
            var randomString = new StringBuilder();
            while (randomString.Length < length)
            {
                randomString.Append(GetRandomCharFromChars(chars));
            }

            return randomString.ToString();
        }
    }
}
