using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Services;
using Moq;
using System;
using Xunit;


namespace Betsson.OnlineWallets.Tests.UnitTests
{
    public class OnlineWalletServicesTests
    {
        private readonly Mock<IOnlineWalletRepository> _mockRepository;
        private readonly OnlineWalletService _service;

        public OnlineWalletServicesTests()
        {
            _mockRepository = new Mock<IOnlineWalletRepository>();
            _service = new OnlineWalletService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetBalanceShouldReturnZeroWhenNoOnlineWalletEntryExist()
        {
            //      Arrange
            //  Set up the mock to return a wallet entry
            _mockRepository
                .Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                .ReturnsAsync((OnlineWalletEntry)null);

            //      Act
            //  Call the GetBalance method
            var result = await _service.GetBalanceAsync();

            //      Assert
            //  Verify that the returned balance is correct
            Assert.NotNull(result);
            Assert.Equal(0, result.Amount);
        }

        [Fact]
        public async Task GetBalanceShouldReturnCorrectBalanceWhenOnlineWalletEntryExists()
        {
            //      Arrange
            //  Set up the mock to return a wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = 50,
                BalanceBefore = 100,
            };
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            // Call the GetBalance method
            var result = await _service.GetBalanceAsync();

            //      Assert
            //  Verify that the returned balance is correct
            Assert.Equal(150, result.Amount);
        }



    }
}
