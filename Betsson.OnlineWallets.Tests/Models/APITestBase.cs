using Betsson.OnlineWallets.Web.Models;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Net.Http.Json;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Betsson.OnlineWallets.Tests.Models
{
    public class APITestBase
    {
        protected readonly HttpClient _client;
        protected readonly ITestOutputHelper outputHelper;

        public APITestBase(ITestOutputHelper output)
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:3000/") };
            outputHelper = output;
        }

        // Method to get balance in every tests class
        public async Task<BalanceResponse> GetBalance()
        {
            var response = await _client.GetAsync(RequestHelper.BalanceEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get balance. Status Code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<BalanceResponse>() ?? new BalanceResponse();
        }
    }
}
