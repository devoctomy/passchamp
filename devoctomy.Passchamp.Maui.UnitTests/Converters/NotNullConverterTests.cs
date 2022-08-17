using devoctomy.Passchamp.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Converters
{
    public class NotNullConverterTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData(123, true)]
        [InlineData("", true)]
        public void GivenValue_WhenConvert_ThenExpectedValueReturned(
            object value,
            bool expectedResult)
        {
            // Arrange
            var sut = new NotNullConverter();

            // Act
            var result = sut.Convert(
                value,
                null,
                null,
                null);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GivenAnyParams_WhenConvertBack_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var sut = new NotNullConverter();

            // Act & Assert
            Assert.ThrowsAny<NotImplementedException>(() =>
            {
                sut.ConvertBack(
                    null,
                    null,
                    null,
                    null);
            });
        }
    }
}
