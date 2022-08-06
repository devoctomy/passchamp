using devoctomy.Passchamp.Maui.Converters;
using System;

namespace devoctomy.Passchamp.Maui.UnitTests.Converters
{
    public class ProviderServiceTypeIdToDisplayNameConverterTests
    {
        [Theory]
        [InlineData("76EEB72B-28DB-49E5-BE25-A2B625BAB333", "Amazon S3 Cloud Storage Provider")]
        public void GivenCloudProviderId_WhenConvert_ThenCloudProviderDisplayNameReturned(
            string cloudProviderId,
            string expectedDisplayName)
        {
            // Arrange
            var sut = new ProviderServiceTypeIdToDisplayNameConverter();

            // Act
            var result = sut.Convert(cloudProviderId, null, null, null);

            // Assert
            Assert.Equal(expectedDisplayName, result);
        }

        [Fact]
        public void GivenAnyParams_WhenConvertBack_ThenNotImplementedExceptionThrown()
        {
            // Arrange
            var sut = new ProviderServiceTypeIdToDisplayNameConverter();

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
