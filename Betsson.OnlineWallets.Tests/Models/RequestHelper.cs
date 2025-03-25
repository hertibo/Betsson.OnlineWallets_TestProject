using Betsson.OnlineWallets.Web.Models;

namespace Betsson.OnlineWallets.Tests.Models
{
    public class RequestHelper
    {
        public const string BalanceEndpoint = "onlinewallet/balance";
        public const string DepositEndpoint = "onlinewallet/deposit";
        public const string WithdrawalEndpoint = "onlinewallet/withdraw";

        public static DepositRequest CreateDepositRequest(decimal amount)
        {
            return new DepositRequest
            {
                Amount = amount
            };
        }
        public static WithdrawalRequest CreateWithdrawalRequest(decimal amount)
        {
            return new WithdrawalRequest
            {
                Amount = amount
            };
        }
    }
}
}
