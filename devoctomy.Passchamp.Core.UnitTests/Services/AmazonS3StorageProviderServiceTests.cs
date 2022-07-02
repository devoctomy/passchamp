using Amazon.S3;
using Amazon.S3.Model;
using devoctomy.Passchamp.Core.Services;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Services
{
    public class AmazonS3StorageProviderServiceTests
    {
        [Fact]
        public async Task GivenPath_AndExists_WhenGetFileInfoAsync_ThenEntryReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";         

            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                ETag = Guid.NewGuid().ToString(),
                LastModified = DateTime.Now
            };

            mockConfig.SetupGet(x => x.Path).Returns("pop/");
            mockConfig.SetupGet(x => x.Bucket).Returns("bucket");
            var expectedPath = $"{mockConfig.Object.Path}{path}";

            mockS3Client.Setup(x => x.GetObjectMetadataAsync(
                It.IsAny<GetObjectMetadataRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.GetFileInfoAsync(
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.HttpStatusCode);
            Assert.Equal(response.LastModified, result.Value.LastModified);
            Assert.Equal(path, result.Value.Name);
            Assert.Equal(expectedPath, result.Value.Path);
            Assert.False(result.Value.IsFolder);
            Assert.Equal(response.ETag.ToUpper(), result.Value.Hash);
            mockS3Client.Verify(x => x.GetObjectMetadataAsync(
                It.Is<GetObjectMetadataRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == path),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenPath_AndNotFound_WhenGetFileInfoAsync_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";
            
            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.NotFound
            };

            mockConfig.SetupGet(x => x.Path).Returns("pop/");
            mockConfig.SetupGet(x => x.Bucket).Returns("bucket");

            mockS3Client.Setup(x => x.GetObjectMetadataAsync(
                It.IsAny<GetObjectMetadataRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.GetFileInfoAsync(
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.Null(result.Value);
            mockS3Client.Verify(x => x.GetObjectMetadataAsync(
                It.Is<GetObjectMetadataRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == path),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenPath_AndUnknownClientException_WhenGetFileInfoAsync_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";

            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.NotFound
            };

            mockConfig.SetupGet(x => x.Path).Returns("pop/");
            mockConfig.SetupGet(x => x.Bucket).Returns("bucket");

            mockS3Client.Setup(x => x.GetObjectMetadataAsync(
                It.IsAny<GetObjectMetadataRequest>(),
                It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    throw new AmazonS3Exception("Something went wrong!");
                });

            // Act
            var result = await sut.GetFileInfoAsync(
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Null(result.HttpStatusCode);
            Assert.Null(result.Value);
            mockS3Client.Verify(x => x.GetObjectMetadataAsync(
                It.Is<GetObjectMetadataRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == path),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenBucketContainingFiles_WhenListFilesAsync_ThenListOfEntriesReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

            var response = new ListObjectsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                S3Objects = new System.Collections.Generic.List<S3Object>
                {
                    new S3Object
                    {
                        Key = "somefolder/"
                    },
                    new S3Object
                    {
                        Key = "somefolder/somefile.ext",
                        ETag = Guid.NewGuid().ToString(),
                        LastModified = DateTime.Now
                    }
                }
            };

            mockS3Client.Setup(x => x.ListObjectsAsync(
                It.IsAny<ListObjectsRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.ListFilesAsync(cancellationTokenSource.Token);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.NotNull(result.Value.SingleOrDefault(x =>
                x.IsFolder &&
                x.Name == "somefolder"));
            Assert.NotNull(result.Value.SingleOrDefault(x =>
                !x.IsFolder &&
                x.Name == "somefile.ext" &&
                x.Path == "somefolder/somefile.ext" &&
                x.Hash == response.S3Objects.Last().ETag.ToUpper() &&
                x.LastModified == response.S3Objects.Last().LastModified));
        }

        [Fact]
        public async Task GivenInaccessibleBucket_WhenListFilesAsync_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

            var response = new ListObjectsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };

            mockS3Client.Setup(x => x.ListObjectsAsync(
                It.IsAny<ListObjectsRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.ListFilesAsync(cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GivenUnknownClientException_WhenListFilesAsync_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3StorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

            var response = new ListObjectsResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
            };

            mockS3Client.Setup(x => x.ListObjectsAsync(
                It.IsAny<ListObjectsRequest>(),
                It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    throw new AmazonS3Exception("Something went wrong!");
                });

            // Act
            var result = await sut.ListFilesAsync(cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Null(result.HttpStatusCode);
            Assert.Null(result.Value);
        }
    }
}
