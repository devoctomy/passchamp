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
    [InlineData(16384, 8, 1, "Hello", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "fhFsF5V3iWlqYi/DuTGBIs7b+qWd9dviMbwQNu2EMVg=")]
    [InlineData(16384, 8, 1, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "eWTgFaLzBp+njXgTOmvELrUrLhwp/PRWdsAwHOU+t10=")]
    [InlineData(16384, 8, 4, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "JLqWyi0pQ018GizmlGEXRSCgCQ3IrBvHteTNO23InII=")]
    [InlineData(1024, 8, 4, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "mINyTQL9SEgx4VYBsZVavbAjMHlCn0ThdR1yr3Ot2ag=")]
    [InlineData(1024, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "wNhgh+c/dkcAHpAxEXDo9bG1eXjWVAfzLEYr+kMy7gY=")]
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
