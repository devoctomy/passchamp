using devoctomy.Passchamp.Maui.Converters;
using devoctomy.Passchamp.Maui.UnitTests.Converters.Enums;
using System;

namespace devoctomy.Passchamp.Maui.UnitTests.Converters
{
    public class EnumEqualityConverterTests
    {
        [Theory]
        [InlineData(TestEnum.Value1, "Value1", true)]
        [InlineData(TestEnum.Value2, "Value2", true)]
        [InlineData(TestEnum.Value1, "Value2", false)]
        public void GivenEnum_AndStringValue_WhenConvert_ThenExpectedResultReturned(
            TestEnum testEnum,
            string value,
            bool expectedResult)
        {
            // Arrange
            var sut = new EnumEqualityConverter();

            // Act
            var result = sut.Convert(
                testEnum,
                null,
                value,
                null);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GivenAnyParams_WhenConvertBack_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var sut = new EnumEqualityConverter();

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
