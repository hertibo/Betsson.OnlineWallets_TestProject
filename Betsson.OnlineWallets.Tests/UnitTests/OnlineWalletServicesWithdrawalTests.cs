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
            const decimal amount = 100;
            const decimal balanceBefore = 200;
            const decimal withdrawAmount = 50;

            // Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
                EventTime = DateTimeOffset.Now
            };

            //  Create a request request
            var request = new Withdrawal { Amount = withdrawAmount };

            // Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the WithdrawFundsAsync method to process the request
            var result = await _service.WithdrawFundsAsync(request);

            //      Assert
            // Verify that the new balance is correct
            Assert.Equal(amount+balanceBefore-withdrawAmount, result.Amount);
        }

        [Fact]
        public async Task WithdrawZeroAmountShouldNotChangeBalance()
        {
            //       Arrange
            const decimal amount = 100;
            const decimal balanceBefore = 200;
            const decimal withdrawAmount = 0;
            
            //  Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
                EventTime = DateTimeOffset.Now
            };
            
            //Create a withdrawal request 
            var request = new Withdrawal { Amount = withdrawAmount };

            // Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the WithdrawFundsAsync method to process the request
            var result = await _service.WithdrawFundsAsync(request);

            //       Assert
            //  Verify that the new balance is correct
            Assert.Equal(amount+ balanceBefore-withdrawAmount, result.Amount); 
        }

        [Fact]
        public async Task WithdrawRequestShouldThrowInsufficientBalanceExceptionInCaseOfInsufficientBalance()
        {
            //      Arrange
            const decimal amount = 50;
            const decimal balanceBefore = 100;
            const decimal withdrawAmount = 200;

            //   Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
                EventTime = DateTimeOffset.Now
            };

            //  Create a withdrawal request 
            var request = new Withdrawal { Amount = withdrawAmount};

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
            const decimal withdrawAmount = 50;

            //  Mock repository to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync((OnlineWalletEntry?)null);

            //  Create a withdrawal request 
            var request = new Withdrawal { Amount = withdrawAmount };

            //       Act & Assert
            //  Verify that an exception is thrown for insufficient balance
            await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.WithdrawFundsAsync(request));
        }
    }
}
