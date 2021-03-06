using devoctomy.Passchamp.Windows.ValueConverters;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using Xunit;

namespace devoctomy.Passchamp.Windows.UnitTests.ValueConverters
{
    public class SecureStringValueConverterTests
    {
        [Fact]
        public void GivenString_WhenConvert_ThenSecureStringReturned_AndSecureStringCorrectValue()
        {
            // Arrange
            var value = "Hello World!";
            var sut = new SecureStringValueConverter();

            // Act
            var result = sut.Convert(
                value,
                null,
                null,
                null);

            // Assert
            Assert.IsType<SecureString>(result);
            var secureString = result as SecureString;
            Assert.Equal(value, new NetworkCredential(null, secureString).Password);
        }
    }
}
