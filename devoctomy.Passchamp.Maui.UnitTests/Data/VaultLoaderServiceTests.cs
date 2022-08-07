﻿using devoctomy.Passchamp.Client.Models;
using devoctomy.Passchamp.Core.Data;
using devoctomy.Passchamp.Maui.Data;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Maui.UnitTests.Data
{
    public class VaultLoaderServiceTests
    {
        [Fact]
        public async Task GivenOptions_WhenLoadAsync_ThenPathCreated_AndFileExists_AndJsonReadFromCorrectPath_AndVaultsDeserialisedCorrectly()
        {
            // Arrange
            var mockIoService = new Mock<IIOService>();
            var options = new VaultLoaderServiceOptions
            {
                Path = "folder1/folder2/",
                FileName = "somefile.json"
            };
            var sut = new VaultLoaderService(
                options,
                mockIoService.Object);

            var expectedVaults = new List<VaultIndex>
            {
                new VaultIndex
                {
                    Id = Guid.NewGuid().ToString()
                },
                new VaultIndex
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            var cancellationTokenSource = new CancellationTokenSource();

            mockIoService.Setup(x => x.CreatePathDirectory(
                It.IsAny<string>()));

            mockIoService.Setup(x => x.Exists(
                It.IsAny<string>()))
                .Returns(true);

            mockIoService.Setup(x => x.ReadAllTextAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(JsonConvert.SerializeObject(expectedVaults));

            // Act
            await sut.LoadAsync(cancellationTokenSource.Token);

            // Assert
            var expectedPath = $"{options.Path}{options.FileName}";
            mockIoService.Verify(x => x.CreatePathDirectory(
                It.Is<string>(y => y == expectedPath)), Times.Once);
            mockIoService.Setup(x => x.Exists(
                It.Is<string>(y => y == expectedPath)))
                .Returns(true);
            mockIoService.Verify(x => x.ReadAllTextAsync(
                It.Is<string>(y => y == expectedPath),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Once);
            Assert.Equal(expectedVaults.Count, sut.Vaults.Count);
            Assert.Contains(sut.Vaults, x => x.Id == expectedVaults[0].Id);
            Assert.Contains(sut.Vaults, x => x.Id == expectedVaults[1].Id);
        }

        [Fact]
        public async Task GivenOptions_WhenLoadAsync_ThenPathCreated_AndFileNotExists_AndNothingLoaded()
        {
            // Arrange
            var mockIoService = new Mock<IIOService>();
            var options = new VaultLoaderServiceOptions
            {
                Path = "folder1/folder2/",
                FileName = "somefile.json"
            };
            var sut = new VaultLoaderService(
                options,
                mockIoService.Object);

            var expectedVaults = new List<VaultIndex>
            {
                new VaultIndex
                {
                    Id = Guid.NewGuid().ToString()
                },
                new VaultIndex
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            var cancellationTokenSource = new CancellationTokenSource();

            mockIoService.Setup(x => x.CreatePathDirectory(
                It.IsAny<string>()));

            mockIoService.Setup(x => x.Exists(
                It.IsAny<string>()))
                .Returns(false);

            // Act
            await sut.LoadAsync(cancellationTokenSource.Token);

            // Assert
            var expectedPath = $"{options.Path}{options.FileName}";
            mockIoService.Verify(x => x.CreatePathDirectory(
                It.Is<string>(y => y == expectedPath)), Times.Once);
            mockIoService.Setup(x => x.Exists(
                It.Is<string>(y => y == expectedPath)))
                .Returns(true);
            mockIoService.Verify(x => x.ReadAllTextAsync(
                It.Is<string>(y => y == expectedPath),
                It.Is<CancellationToken>(y => y == cancellationTokenSource.Token)), Times.Never);
            Assert.Empty(sut.Vaults);
        }
    }
}
