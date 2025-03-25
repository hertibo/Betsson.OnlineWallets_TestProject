using Betsson.OnlineWallets.Tests.Models;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace Betsson.OnlineWallets.Tests.APITests
{
    public class DepositApiTests : TestBase
    {
        [Fact]
        public async Task ValidateDepositResponseSuccessfulStatusCodeWithPositiveAmount()
        {
            //      Arrange
            //  Prepare a deposit request with a positive amount
            decimal positiveAmount = 100;
            var request = RequestHelper.CreateDepositRequest(positiveAmount);

            //      Act
            //  Send the deposit request
            var response = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, request);

            //      Assert
            //  Ensure that the deposit response returns a 200 status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ValidateDepositResponseSuccessfulStatusCodeWithZeroAmount()
        {
            //      Arrange
            //  Prepare a deposit request with zero amount
            decimal zeroAmount = 0;
            var depositRequest = RequestHelper.CreateDepositRequest(zeroAmount);

            //      Act
            //  Send the deposit request
            var response = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);

            //      Assert
            //  Ensure that the deposit response returns a 200 status code
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ValidateDepositResponeBadStatusCodeWithNegativeAmount()
        {
            //      Arrange
            //  Prepare a deposit request with a negative amount
            decimal negativeAmont = -1;
            var request = RequestHelper.CreateDepositRequest(negativeAmont);

            //      Act
            //  Send the deposit request
            var response = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, request);

            //      Assert
            //  Ensure that the deposit response returns a 400 status code
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ValidateDepositResponeBadStatusCodeWithInvalidFormat()
        {
            //      Arrange
            //  Prepare a deposit request with a negative amount
            string stringAmount = "hundred";
            var request = new { Amount = stringAmount };

            //      Act
            //  Send the deposit request
            var response = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, request);

            //      Assert
            //  Ensure that the deposit response returns a 400 status code
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
