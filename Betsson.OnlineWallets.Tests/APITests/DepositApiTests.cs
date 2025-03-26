using Betsson.OnlineWallets.Tests.Models;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace Betsson.OnlineWallets.Tests.APITests
{
    public class DepositApiTests : APITestBase
    {
        public DepositApiTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task ValidateDepositResponseSuccessfulStatusCodeWithPositiveAmount()
        {
            //      Arrange
            const decimal positiveAmount = 100;

            //  Prepare a deposit request with a positive amount
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
            const decimal zeroAmount = 0;

            //  Prepare a deposit request with zero amount
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
            const decimal negativeAmont = -1;

            //  Prepare a deposit request with a negative amount
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
            const string stringAmount = "hundred";

            //  Prepare a deposit request with a negative amount
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
