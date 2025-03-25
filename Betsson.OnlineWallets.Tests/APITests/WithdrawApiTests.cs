using Betsson.OnlineWallets.Tests.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Betsson.OnlineWallets.Tests.APITests
{
    public class WithdrawApiTests : TestBase
    {
        [Fact]
        public async Task ValidateWithdrawalResponseSuccessfulStatusCodeWithPositiveAmount()
        {
            //      Arrange
            //  Prepare a withdrawal request with a positive amount
            decimal positiveAmount = 100;
            var request = RequestHelper.CreateWithdrawalRequest(positiveAmount);

            //      Act
            //  Send the withdrawal request
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, request);

            //      Assert
            //  Ensure that the withdrawal response returns a 200 status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ValidateWithdrawalResponseSuccessfulStatusCodeWithZeroAmount()
        {
            //      Arrange
            //  Prepare a withdrawal request with zero amount
            decimal zeroAmount = 0;
            var request = RequestHelper.CreateWithdrawalRequest(zeroAmount);

            //      Act
            //  Send the withdrawal request
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, request);

            //      Assert
            //  Ensure that the withdrawal response returns a 200 status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ValidateWithdrawalResponeBadStatusCodeWithNegativeAmount()
        {
            //      Arrange
            //  Prepare a withdrawal request with negative amount
            decimal negativeAmont = -1;
            var request = RequestHelper.CreateWithdrawalRequest(negativeAmont);

            //      Act
            //  Send the withdrawal request
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, request);

            //      Assert
            //  Ensure that the withdrawal response returns a 400 status code
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ValidateWithdrawalResponeBadStatusCodeWithInvalidFormat()
        {
            //      Arrange
            //  Prepare a withdrawal request with a negative amount
            string stringAmount = "ten";
            var request = new { Amount = stringAmount };

            //      Act
            //  Send the withdrawal request
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, request);

            //      Assert
            //  Ensure that the withdrawal response returns a 400 status code
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
