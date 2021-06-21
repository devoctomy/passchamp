using devoctomy.Passchamp.Core.Graph;
using devoctomy.Passchamp.Core.Graph.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace devoctomy.Passchamp.Core.UnitTests.Graph.Cryptography
{
    public class SCryptNodeTests
    {
        [Theory]
        [InlineData(16384, 8, 1, "Hello", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "lMjS4eTH4PwO/35o2PrEPeiocyUkAR/COhtnUQaWJzE=")]
        [InlineData(16384, 8, 1, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "lct25iWaiw6eTPE9PA5VRsHoSJvPN4ec34durDGxd7k=")]
        [InlineData(16384, 8, 4, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "urIahQMbCigEc/aqPCKBgbr1EQ9mL9EJuh/+5SWEI38=")]
        [InlineData(1024, 8, 4, "Password123", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "c8B8czWegRIRrH0aQ2B9MgQB8gIcT4huqhPNFLfZ9V0=")]
        [InlineData(1024, 8, 4, "Password123", new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "Z5UpdzTCXzI5sUHtURhvCtf8r8lJx0V3+ko0Wd967Vs=")]
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
            var sut = new SCryptNode
            {
                IterationCount = new DataPin(iterationCount),
                BlockSize = new DataPin(blockSize),
                ThreadCount = new DataPin(threadCount),
                Password = new DataPin(password),
                Salt = new DataPin(salt),
            };
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await sut.Execute(
                mockGraph.Object,
                cancellationTokenSource.Token);

            // Assert
            var actualKeyBase64 = Convert.ToBase64String(sut.Key.GetValue<byte[]>());
            Assert.Equal(keyBase64, actualKeyBase64);
        }
    }
}
