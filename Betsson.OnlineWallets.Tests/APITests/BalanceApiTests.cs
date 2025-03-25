using Betsson.OnlineWallets.Tests.Models;
using FluentAssertions;
using System.Net;
using Xunit.Abstractions;

namespace Betsson.OnlineWallets.Tests.APITests
{
    public class BalanceApiTests : TestBase
    {
        public BalanceApiTests(ITestOutputHelper output) : base(output) { }
    
        [Fact]
        public async Task ValidateBalanceResponseSuccessfulCode()
        {
            //      Act
            // Send the balance request
            var response = await _client.GetAsync(RequestHelper.BalanceEndpoint);

            //      Assert
            // Ensure that the response returns a 200 status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ValidateBalanceAmountIsNonNegative()
        {
            //      Arrange
            //  Get the balance from the API
            var balance = await GetBalance();

            // Assert
            // Ensure the balance object is not null
            balance.Should().NotBeNull();

            // Ensure that the balance amount is zero or greater 
            balance.Amount.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}
