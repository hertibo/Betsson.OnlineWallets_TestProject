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
            
            // Set up the mock to return the last wallet entry
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
