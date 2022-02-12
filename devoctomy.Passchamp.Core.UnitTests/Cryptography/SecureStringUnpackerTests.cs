using devoctomy.Passchamp.Core.Cryptography;
using System;
using System.Net;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cryptography
{
    public class SecureStringUnpackerTests
    {
        [Fact]
        public void GivenSecureString_WhenUnpack_ThenCallbackCalledWithPlainText()
        {
            // Arrange
            var plainText = "Hello World";
            var secureString = new NetworkCredential(null, plainText).SecurePassword;
            var sut = new SecureStringUnpacker();
            var result = string.Empty;

            Action<byte[]> callback = buffer =>
            {
                result = System.Text.Encoding.Unicode.GetString(buffer);
            };

            // Act
            sut.Unpack(secureString, callback);

            // Assert
            Assert.Equal(plainText, result);
        }
    }
}
