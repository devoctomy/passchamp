using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using devoctomy.Passchamp.Core.Cloud.Utility;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cloud.Utility
{
    public class CloudStorageProviderServiceAttributeUtilityTests
    {
        [Fact]
        public void GivenUnknownType_WhenGet_ThenNullReturned()
        {
            // Act
            var result = CloudStorageProviderServiceAttributeUtility.Get(typeof(AmazonS3CloudStorageProviderConfig));

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(typeof(AmazonS3CloudStorageProviderService), "76EEB72B-28DB-49E5-BE25-A2B625BAB333")]
        public void GivenCorrectType_WhenGet_ThenAttributeReturned(
            Type type,
            string expectedTypeId)
        {
            // Act
            var result = CloudStorageProviderServiceAttributeUtility.Get(type);

            // Assert
            Assert.Equal(expectedTypeId, result.TypeId);
        }

        [Theory]
        [InlineData("76EEB72B-28DB-49E5-BE25-A2B625BAB333")]
        public void GivenTypeId_WhenGet_ThenAttributeReturned(string typeId)
        {
            // Act
            var result = CloudStorageProviderServiceAttributeUtility.Get(typeId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GivenUnknownTypeId_WhenGet_ThenInvalidOperationExceptionThrown()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = CloudStorageProviderServiceAttributeUtility.Get("Unknown TypeId");
            });
        }
    }
}
