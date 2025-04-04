﻿using Betsson.OnlineWallets.Exceptions;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Tests.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Betsson.OnlineWallets.Tests.APITests
{
    public class EndToEndTests : APITestBase
    {
        public EndToEndTests(ITestOutputHelper output) : base(output) { }
        
        [Fact]
        public async Task DepositSuccessfulEndToEndFlow()
        {
            //      Arrange
            const decimal amountToDeposit = 150;

            //  Get initial balance
            var balance = await GetBalance();
            decimal initialBalance = balance.Amount;

            //  Create Deposit request
            var depositRequest = RequestHelper.CreateDepositRequest(amountToDeposit);

            //      Act
            //  Send a request
            var response = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);

            //      Assert
            //  Assert that the response has a successful status code
            response.EnsureSuccessStatusCode();

            //  Deserialize the response and ensure it's not null
            var depositResponse = await response.Content.ReadFromJsonAsync<Deposit>();
            depositResponse.Should().NotBeNull();

            //  Assert that the returned balance is correct (initial amount + deposited amount)
            depositResponse!.Amount.Should().Be(initialBalance + amountToDeposit);
            // Double check it with balance endpoint
            balance = await GetBalance();
            balance!.Amount.Should().Be(depositResponse!.Amount);
            outputHelper.WriteLine($"Deposit response: {depositResponse!.Amount} , initial amount + deposited amount: {initialBalance + amountToDeposit}");
        }

        [Fact]
        public async Task MultipleDepositSuccessfulEndToEndFlow()
        {
            //      Arrange
            int depositTimes = 4;
            const decimal amountToDeposit = 8;

            //  Get initial balance
            var balance = await GetBalance();
            decimal initialBalance = balance.Amount;

            //      Act
            //  Create deposit request
            var depositRequest = RequestHelper.CreateDepositRequest(amountToDeposit);
            //  Send deposit request multiple times
            for (int i = 0; i < depositTimes; i++)
            {
                await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);
            }

            //      Assert
            //  Check if the balance has increased
            var finalBalance = await GetBalance();
            finalBalance.Amount.Should().Be(initialBalance + (amountToDeposit * depositTimes));
            outputHelper.WriteLine($"Balance response: {finalBalance.Amount} , Initial amount + deposited amount: {initialBalance + (amountToDeposit * depositTimes)}");
        }


        [Fact]
        public async Task WithdrawalSuccessfulEndToEndFlow()
        {
            //      Arrange
            const decimal amountToWithdraw = 85;

            //  Get initial balance
            var balance = await GetBalance();
            decimal initialBalance = balance.Amount;

            //  Ensure there is enough balance before proceeding
            if (initialBalance < amountToWithdraw)
            {
                // Deposit sufficient credit in case of insufficient initial balance
                var depositRequest = RequestHelper.CreateDepositRequest(amountToWithdraw);
                var depositResponse = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);
                // Assert that the deposit response was successful
                depositResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Test failed: Unable to deposit sufficient credit.");
                // Update the inital balance for the assertion
                initialBalance = balance.Amount + amountToWithdraw;
            }

            //  Create Withdrawal request
            var withdrawalRequest = RequestHelper.CreateWithdrawalRequest(amountToWithdraw);
            //      Act
            //  Send the request 
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, withdrawalRequest);

            //      Assert
            //  Assert that the response has a successful status code
            response.EnsureSuccessStatusCode();

            //  Deserialize the response and ensure it's not null
            var withdrawalResponse = await response.Content.ReadFromJsonAsync<Withdrawal>();
            withdrawalResponse.Should().NotBeNull("Withdrawal response should not be null.");

            //  Check if the withdrawal amount is correct
            withdrawalResponse!.Amount.Should().Be(initialBalance - amountToWithdraw);
            // Double check it with balance endpoint
            balance = await GetBalance();
            balance!.Amount.Should().Be(withdrawalResponse!.Amount);
            outputHelper.WriteLine($"Withdrawal response: {withdrawalResponse!.Amount} , Initial amount - amountToWithdraw: {initialBalance - amountToWithdraw}");
        }
        

        [Fact]
        public async Task MultipleWithdrawalSuccessfulEndToEndFlowd()
        {
            //      Arrange
            int withdrawalTimes = 3;
            const decimal amountToWithdraw = 50;

            //  Get initial balance
            var balance = await GetBalance();
            decimal initialBalance = balance.Amount;

            //  Ensure there is enough balance before proceeding
            if (initialBalance < (amountToWithdraw * withdrawalTimes))
            {
                // Deposit sufficient credit in case of insufficient initial balance
                var depositRequest = RequestHelper.CreateDepositRequest(amountToWithdraw * withdrawalTimes);
                var depositResponse = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);
                // Assert that the deposit response was successful
                depositResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Test failed: Unable to deposit sufficient credit.");
                // Update the inital balance for the assertion
                initialBalance = balance.Amount + (amountToWithdraw * withdrawalTimes);
            }

            //      Act
            //  Create withdrawal request
            var withdrawalRequest = RequestHelper.CreateWithdrawalRequest(amountToWithdraw);
            //  Send request multiple times
            for (int i = 0; i < withdrawalTimes; i++)
            {
                await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, withdrawalRequest);
            }

            //      Assert
            //  Check if the balance has decreased
            var finalBalance = await GetBalance();
            finalBalance.Amount.Should().Be(initialBalance - (amountToWithdraw * withdrawalTimes));
            outputHelper.WriteLine($"Balance response: {finalBalance.Amount} , Initial amount - (amountToWithdraw * withdrawalTimes): {initialBalance - (amountToWithdraw * withdrawalTimes)}");
        }

        [Fact]
        public async Task WithdrawAllAvailableBalanceEndToEndFlow()
        {
            //      Arrange
            //  Get initial balance
            var balance = await GetBalance();
            decimal balanceAmount = balance.Amount;

            //  Create Withdrawal request
            var withdrawalRequest = RequestHelper.CreateWithdrawalRequest(balanceAmount);

            //      Act
            //  Send the request 
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, withdrawalRequest);

            //      Assert
            //  Assert that the response has a successful status code
            response.EnsureSuccessStatusCode();

            //  Deserialize the response and ensure it's not null
            var withdrawalResponse = await response.Content.ReadFromJsonAsync<Withdrawal>();
            withdrawalResponse.Should().NotBeNull("Withdrawal response should not be null.");

            //  Check if the withdrawal amount is correct
            withdrawalResponse!.Amount.Should().Be(0);
            // Double check it with balance endpoint
            balance = await GetBalance();
            balance!.Amount.Should().Be(0);
            outputHelper.WriteLine($"Balance response: {balance!.Amount}");
        }

        [Fact]
        public async Task WithdrawSlightlyMoreThanAvailableBalance()
        {
            //      Arrange
            //  Get initial balance
            var balance = await GetBalance();
            decimal actualBalance = balance.Amount;
            decimal minimumAmount = 10;
            decimal surplusAmount = 0.0001m;

            //  Ensure there is enough balance before proceeding
            if (actualBalance < minimumAmount)
            {
                // Deposit sufficient credit in case of insufficient initial balance
                var depositRequest = RequestHelper.CreateDepositRequest(minimumAmount);
                var depositResponse = await _client.PostAsJsonAsync(RequestHelper.DepositEndpoint, depositRequest);
                // Assert that the deposit response was successful
                depositResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Test failed: Unable to deposit sufficient credit.");
            }
            // Update the actualBalance balance for the assertion
            balance = await GetBalance();
            actualBalance = balance.Amount;
            
            //  Create Withdrawal request
            var withdrawalRequest = RequestHelper.CreateWithdrawalRequest(actualBalance+ surplusAmount);
            outputHelper.WriteLine($"Actual balance: {actualBalance} , amount try to withdraw: {actualBalance + surplusAmount}");

            //      Act
            //  Send the request 
            var response = await _client.PostAsJsonAsync(RequestHelper.WithdrawalEndpoint, withdrawalRequest);

            //      Assert
            // Check if the API rejects the request with a 400 Bad Request
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 

            // Check if the error message contains "insufficient funds"
            var errorMessage = await response.Content.ReadAsStringAsync();
            errorMessage.Should().Contain("insufficient funds", "The error message should contains insufficient funds.");

            //  Check if the error message mentions InsufficientBalanceException
            errorMessage.Should().Contain(nameof(InsufficientBalanceException));
        }
    }
}
