using Betsson.OnlineWallets.Web.Models;
using System.Net.Http.Json;

namespace Betsson.OnlineWallets.Tests.Models
{
    public class TestBase
    {
        protected readonly HttpClient _client;

        public TestBase()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:3000/") };
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
