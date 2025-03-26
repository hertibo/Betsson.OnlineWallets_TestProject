using Betsson.OnlineWallets.Data.Repositories;
using Betsson.OnlineWallets.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betsson.OnlineWallets.Tests.Models
{
    public class UnitTestBase
    {
        public readonly Mock<IOnlineWalletRepository> _mockRepository;
        public readonly OnlineWalletService _service;

        public UnitTestBase()
        {
            _mockRepository = new Mock<IOnlineWalletRepository>();
            _service = new OnlineWalletService(_mockRepository.Object);
        }
    }
}
