using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Tests.Models;
using Moq;


namespace Betsson.OnlineWallets.Tests.UnitTests
{
    public class OnlineWalletServicesBalanceTests : UnitTestBase
    {

        [Fact]
        public async Task GetBalanceShouldReturnZeroWhenNoOnlineWalletEntryExist()
        {
            //      Arrange
            // Set up the mock to return the last wallet entry
            _mockRepository
                .Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                .ReturnsAsync((OnlineWalletEntry?)null);

            //      Act
            //  Call the GetBalanceAsync method
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
            const decimal amount = 50;
            const decimal balanceBefore = 100;

            //  Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
            };

            // Set up the mock to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the GetBalance method
            var result = await _service.GetBalanceAsync();

            //      Assert
            //  Verify that the returned balance is correct
            Assert.Equal(amount+balanceBefore, result.Amount);
        }
    }
}
