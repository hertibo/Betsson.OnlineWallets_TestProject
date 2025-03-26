using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Services;
using Moq;

namespace Betsson.OnlineWallets.Tests.UnitTests
{
    public class OnlineWalletServicesTests
    {
        private readonly Mock<IOnlineWalletRepository> _mockRepository;
        private readonly OnlineWalletService _service;

        public OnlineWalletServicesTests()
        {
            _mockRepository = new Mock<IOnlineWalletRepository>();
            _service = new OnlineWalletService(_mockRepository.Object);
        }



    }
}
