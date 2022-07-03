using Amazon.S3;
using Amazon.S3.Model;
using devoctomy.Passchamp.Core.Cloud;
using devoctomy.Passchamp.Core.Cloud.AmazonS3;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Cloud.AmazonS3
{
    public class AmazonS3CloudStorageProviderServiceTests
    {
        [Fact]
        public async Task GivenPath_AndExists_WhenGetFileInfoAsync_ThenEntryReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";

            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = HttpStatusCode.OK,
                ETag = Guid.NewGuid().ToString(),
                LastModified = DateTime.Now
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");
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
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
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
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";

            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = HttpStatusCode.NotFound
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");

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
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
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
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var path = "folder1/folder2/filename.ext";

            var response = new GetObjectMetadataResponse
            {
                HttpStatusCode = HttpStatusCode.NotFound
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");

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
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

            var response = new ListObjectsResponse
            {
                HttpStatusCode = HttpStatusCode.OK,
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
        public async Task GivenUnauthorised_WhenListFilesAsync_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

            var response = new ListObjectsResponse
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
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
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();

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

        [Fact]
        public async Task GivenDataStream_AndPath_WhenPutFile_ThenSuccessReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            using var data = new MemoryStream();
            var path = "somefolder/somefile.ex";

            var response = new PutObjectResponse
            {
                HttpStatusCode = HttpStatusCode.OK
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");
            var fullPath = $"{mockConfig.Object.Path}{path}";

            mockS3Client.Setup(x => x.PutObjectAsync(
                It.IsAny<PutObjectRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.PutFileAsync(
                data,
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            mockS3Client.Verify(x => x.PutObjectAsync(
                It.Is<PutObjectRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == fullPath &&
                y.InputStream == data &&
                y.AutoCloseStream == true &&
                y.AutoResetStreamPosition == true),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenDataStream_AndPath_AndUnauthorised_WhenPutFile_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            using var data = new MemoryStream();
            var path = "somefolder/somefile.ex";

            var response = new PutObjectResponse
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");
            var fullPath = $"{mockConfig.Object.Path}{path}";

            mockS3Client.Setup(x => x.PutObjectAsync(
                It.IsAny<PutObjectRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await sut.PutFileAsync(
                data,
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatusCode);
            mockS3Client.Verify(x => x.PutObjectAsync(
                It.Is<PutObjectRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == fullPath &&
                y.InputStream == data &&
                y.AutoCloseStream == true &&
                y.AutoResetStreamPosition == true),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }

        [Fact]
        public async Task GivenDataStream_AndPath_AndUnknownClientException_WhenPutFile_ThenErrorReturned()
        {
            // Arrange
            var mockConfig = new Mock<IAmazonS3Config>();
            var mockS3Client = new Mock<IAmazonS3>();
            var sut = new AmazonS3CloudStorageProviderService(
                mockConfig.Object,
                mockS3Client.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            using var data = new MemoryStream();
            var path = "somefolder/somefile.ex";

            var response = new PutObjectResponse
            {
                HttpStatusCode = HttpStatusCode.Unauthorized
            };

            mockConfig
                .SetupGet(x => x.Path)
                .Returns("pop/");
            mockConfig
                .SetupGet(x => x.Bucket)
                .Returns("bucket");
            var fullPath = $"{mockConfig.Object.Path}{path}";

            mockS3Client.Setup(x => x.PutObjectAsync(
                It.IsAny<PutObjectRequest>(),
                It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    throw new AmazonS3Exception("Something went wrong!");
                });

            // Act
            var result = await sut.PutFileAsync(
                data,
                path,
                cancellationTokenSource.Token);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Null(result.HttpStatusCode);
            mockS3Client.Verify(x => x.PutObjectAsync(
                It.Is<PutObjectRequest>(y =>
                y.BucketName == mockConfig.Object.Bucket &&
                y.Key == fullPath &&
                y.InputStream == data &&
                y.AutoCloseStream == true &&
                y.AutoResetStreamPosition == true),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
        }
    }
}
