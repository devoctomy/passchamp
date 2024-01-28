using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using System;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cloud.AmazonS3;

public class AmazonS3CloudStorageProviderConfigTests
{
    [Fact]
    public void GivenConfig_WhenClone_ThenCopyReturned_AndOnlyHashDiffers()
    {
        // Arrange
        var sut = new AmazonS3CloudStorageProviderConfig
        {
            Id = Guid.NewGuid().ToString(),
            AccessId = Guid.NewGuid().ToString(),
            SecretKey = Guid.NewGuid().ToString(),
            Bucket = Guid.NewGuid().ToString(),
            DisplayName = Guid.NewGuid().ToString(),
            Path = Guid.NewGuid().ToString(),
            Region = Guid.NewGuid().ToString(),
        };

        // Act
        var clone = (AmazonS3CloudStorageProviderConfig)sut.Clone();

        // Assert
        Assert.NotEqual(sut.GetHashCode(), clone.GetHashCode());
        Assert.Equal(sut.Id, clone.Id);
        Assert.Equal(sut.AccessId, clone.AccessId);
        Assert.Equal(sut.SecretKey, clone.SecretKey);
        Assert.Equal(sut.Bucket, clone.Bucket);
        Assert.Equal(sut.DisplayName, clone.DisplayName);
        Assert.Equal(sut.Path, clone.Path);
        Assert.Equal(sut.Region, clone.Region);
    }
}
