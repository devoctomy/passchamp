using devoctomy.Passchamp.Core.Cryptography;
using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography;

public class SCryptExNodeTests
{
    [Fact]
    public async Task GivenInvalidIterationCount_WhenExecuteAsync_ThenArgumentOutOfRangeException()
    {
        // Arrange
        var mockGraph = new Mock<IGraph>();
        var sut = new SCryptNode(new SecureStringUnpacker())
        {
            IterationCount = (IDataPin<int>)DataPinFactory.Instance.Create(
                "IterationCount",
                0),
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, "password123").SecurePassword)
        };
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentOutOfRangeException>(async () =>
        {
            await sut.ExecuteAsync(cancellationTokenSource.Token);
        });
    }

    [Fact]
    public async Task GivenInvalidBlockSize_WhenExecuteAsync_ThenArgumentOutOfRangeException()
    {
        // Arrange
        var mockGraph = new Mock<IGraph>();
        var sut = new SCryptNode(new SecureStringUnpacker())
        {
            BlockSize = (IDataPin<int>)DataPinFactory.Instance.Create(
                "BlockSize",
                0),
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, "password123").SecurePassword)
        };
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentOutOfRangeException>(async () =>
        {
            await sut.ExecuteAsync(cancellationTokenSource.Token);
        });
    }

    [Fact]
    public async Task GivenInvalidThreadCount_WhenExecuteAsync_ThenArgumentOutOfRangeException()
    {
        // Arrange
        var mockGraph = new Mock<IGraph>();
        var sut = new SCryptNode(new SecureStringUnpacker())
        {
            ThreadCount = (IDataPin<int>)DataPinFactory.Instance.Create(
                "ThreadCount",
                0),
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, "password123").SecurePassword)
        };
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentOutOfRangeException>(async () =>
        {
            await sut.ExecuteAsync(cancellationTokenSource.Token);
        });
    }

    [Theory]
    // !!! NCrunch has some serious issues running these tests, we need larger tests that run outside of Visual Studio !!!
    [InlineData(8, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "gKwXosgWPeKZ2+jsrYBz2tAliedDX91TrYChkuEQ/bE=")]
    [InlineData(16, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "C0njLCAnTQNgo+d3SZBW6R7Q27+HY8TikuhXNOTgDbQ=")]
    [InlineData(32, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "i5z6hF02Y7FJ3I1GNXi63OQyUrr09oeRK+rVTWB9zx8=")]
    [InlineData(64, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "jYLb2CiA1YCKvAH6n8WshJsi1dZM1Kc0iPmBo8YnhYw=")]
    [InlineData(128, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "X108aGJxxo0wIi60LGLf++hqK49k3GyYPA60DUwHlA4=")]
    public async Task GivenIterationCount_AndBlockSize_AndThreadCount_AndPassword_AndSalt_WhenExecute_ThenExpectedKeyDerived(
        int iterationCount,
        int blockSize,
        int threadCount,
        string password,
        byte[] salt,
        string keyBase64)
    {
        // Arrange
        var mockGraph = new Mock<IGraph>();
        var sut = new SCryptNode(new SecureStringUnpacker())
        {
            IterationCount = (IDataPin<int>)DataPinFactory.Instance.Create(
                "IterationCount",
                iterationCount),
            BlockSize = (IDataPin<int>)DataPinFactory.Instance.Create(
                "BlockSize",
                blockSize),
            ThreadCount = (IDataPin<int>)DataPinFactory.Instance.Create(
                "ThreadCount",
                threadCount),
            SecurePassword = (IDataPin<SecureString>)DataPinFactory.Instance.Create(
                "SecurePassword",
                new NetworkCredential(null, password).SecurePassword),
            Salt = (IDataPin<byte[]>)DataPinFactory.Instance.Create(
                "Salt",
                salt),
        };
        var cancellationTokenSource = new CancellationTokenSource();
        sut.AttachGraph(mockGraph.Object);

        // Act
        await sut.ExecuteAsync(cancellationTokenSource.Token);

        // Assert
        var actualKeyBase64 = Convert.ToBase64String(sut.Key.Value);
        Assert.Equal(keyBase64, actualKeyBase64);
    }
}
