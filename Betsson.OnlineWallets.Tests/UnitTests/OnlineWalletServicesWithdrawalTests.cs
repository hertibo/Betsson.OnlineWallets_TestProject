using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Exceptions;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Tests.Models;
using Moq;

namespace Betsson.OnlineWallets.Tests.UnitTests
{
    public class OnlineWalletServicesWithdrawalTests : UnitTestBase
    {
        [Fact]
        public async Task WithdrawRequestShouldDeductAmountAndReturnNewBalanceInCaseOfSufficientBalance()
        {
            //      Arrange
            // Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = 100,
                BalanceBefore = 200,
                EventTime = DateTimeOffset.Now
            };

            //  Create a request request
            var request = new Withdrawal { Amount = 50 };

            // Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the WithdrawFundsAsync method to process the request
            var result = await _service.WithdrawFundsAsync(request);

            //      Assert
            // Verify that the new balance is correct (Expected new balance is 100+200 - 50 = 250)
            Assert.Equal(250, result.Amount);
        }

        [Fact]
        public async Task WithdrawZeroAmountShouldNotChangeBalance()
        {
            // Arrange
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = 100,
                BalanceBefore = 200,
                EventTime = DateTimeOffset.Now
            };

            var request = new Withdrawal { Amount = 0 };

            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            // Act
            var result = await _service.WithdrawFundsAsync(request);

            // Assert
            Assert.Equal(300, result.Amount); // 100+200 - 0 = 300
        }

        [Fact]
        public async Task WithdrawRequestShouldThrowInsufficientBalanceExceptionInCaseOfInsufficientBalance()
        {
            //      Arrange
            //   Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = 50,
                BalanceBefore = 100,
                EventTime = DateTimeOffset.Now
            };

            //Create a request request
            var request = new Withdrawal { Amount = 200 };

            // Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act & Assert
            //  Verify that an exception is thrown for insufficient balance
            await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.WithdrawFundsAsync(request));
        }

        [Fact]
        public async Task WithdrawRequestShouldThrowInsufficientBalanceExceptionWhenNoOnlineWalletEntryExisty()
        {
            //      Arrange
            //    Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync((OnlineWalletEntry)null);

            //Create a request request
            var request = new Withdrawal { Amount = 50 };

            //       Act & Assert
            //  Verify that an exception is thrown for insufficient balance
            await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.WithdrawFundsAsync(request));
        }
    }
}
