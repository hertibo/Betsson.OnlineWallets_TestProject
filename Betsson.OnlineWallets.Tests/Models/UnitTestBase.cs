using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Services;
using Betsson.OnlineWallets.Web.Validators;
using Moq;

namespace Betsson.OnlineWallets.Tests.Models
{
    public class UnitTestBase
    {
        public readonly Mock<IOnlineWalletRepository> _mockRepository;
        public readonly OnlineWalletService _service;
        public readonly DepositRequestValidator _validator;

        public UnitTestBase()
        {
            _mockRepository = new Mock<IOnlineWalletRepository>();
            _service = new OnlineWalletService(_mockRepository.Object);
            _validator = new DepositRequestValidator();
        }
    }
}
