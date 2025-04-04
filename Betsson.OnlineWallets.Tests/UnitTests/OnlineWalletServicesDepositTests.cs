﻿using Betsson.OnlineWallets.Data.Models;
using Betsson.OnlineWallets.Exceptions;
using Betsson.OnlineWallets.Models;
using Betsson.OnlineWallets.Tests.Models;
using Betsson.OnlineWallets.Web.Models;
using FluentValidation.TestHelper;
using Moq;

namespace Betsson.OnlineWallets.Tests.UnitTests
{
    public class OnlineWalletServicesDepositTests : UnitTestBase
    {
        [Fact]
        public async Task DepositShouldAddAmountAndReturnNewBalance()
        {
            //      Arrange
            const decimal amount = 50;
            const decimal balanceBefore = 100;
            const decimal depositAmount = 50;
            
            //  Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
                EventTime = DateTimeOffset.Now
            };

            // Create the deposit request
            var request = new Deposit { Amount = depositAmount };

            // Set up the mock to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the DepositFundsAsync method to process the deposit
            var result = await _service.DepositFundsAsync(request);

            //      Assert
            //  Verify that the new balance is correct
            Assert.Equal(amount+balanceBefore+depositAmount, result.Amount);
        }

        [Fact]
        public async Task DepositZeroAmountShouldNotChangeBalance()
        {
            //      Arrange
            const decimal amount = 0;
            const decimal balanceBefore = 200;
            const decimal depositAmount = 0;
            
            //  Set up the mock for the last wallet entry
            var lastWalletEntry = new OnlineWalletEntry
            {
                Amount = amount,
                BalanceBefore = balanceBefore,
                EventTime = DateTimeOffset.Now
            };

            // Create the deposit request with an amount of 0
            var request = new Deposit { Amount = depositAmount };

            // Set up the mock to return the last wallet entry
            _mockRepository.Setup(repo => repo.GetLastOnlineWalletEntryAsync())
                           .ReturnsAsync(lastWalletEntry);

            //      Act
            //  Call the DepositFundsAsync method to process the deposit
            var result = await _service.DepositFundsAsync(request);

            //      Assert
            //  Verify that the new balance is correct
            Assert.Equal(amount + balanceBefore + depositAmount, result.Amount);
        }

        [Fact]
        public void DepositWithValidAmountShouldPassValidation()
        {
            //      Arrange
            const decimal positivDepositAmount = 100;

            //  Create a new DepositRequest with a valid positiv amount to test validation
            var request = new DepositRequest { Amount = positivDepositAmount };

            //      Act
            //  Validate the DepositRequest using the validator
            var result = _validator.TestValidate(request);

            //      Assert
            //  Verify that the validation passes for the Amount 
            result.ShouldNotHaveValidationErrorFor(d => d.Amount);
        }

        [Fact]
        public void DepositWithNegativeAmountShouldFailValidation()
        {
            //      Arrange
            const decimal negativeDepositAmount = -50;

            //  Create a new DepositRequest with a negative amount to test validation
            var request = new DepositRequest { Amount = negativeDepositAmount };

            //      Act
            //  Validate the DepositRequest using the validator
            var result = _validator.TestValidate(request);

            //      Assert
            //  Verify that the validation fails for the Amount property when it's negative
            result.ShouldHaveValidationErrorFor(d => d.Amount);
        }
    }
}
