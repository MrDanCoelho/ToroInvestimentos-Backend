using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.Application.Services;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Interfaces.IAdapters;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Services
{
    public class BrokerServiceTest
    {
        private readonly Mock<ILogger<BrokerService>> _logger;
        private readonly Mock<IBankAccountRepository> _bankAccountRepository;
        private readonly Mock<IBankAccountStockRepository> _bankAccountStockRepository;
        private readonly Mock<IBrokerAdapter> _brokerAdapter;
        private readonly Mock<IStockRepository> _stockRepository;

        public BrokerServiceTest()
        {
            _logger = new Mock<ILogger<BrokerService>>();
            _bankAccountRepository = new Mock<IBankAccountRepository>();
            _bankAccountStockRepository = new Mock<IBankAccountStockRepository>();
            _brokerAdapter = new Mock<IBrokerAdapter>();
            _stockRepository = new Mock<IStockRepository>();
        }

        [Fact]
        public async Task GetTrending()
        {
            // Arrange
            var obj = new List<StockEntity>
            {
                new StockEntity(),
                new StockEntity(),
                new StockEntity(),
                new StockEntity(),
                new StockEntity()
            };
            _stockRepository.Setup(x => x.GetAll()).ReturnsAsync(obj);
            
            // Act
            var service = new BrokerService(_logger.Object, _bankAccountRepository.Object,
                _bankAccountStockRepository.Object, _brokerAdapter.Object, _stockRepository.Object);
            var result = await service.GetTrending();
            
            // Assert
            Assert.NotEmpty(result);
        }
    }
}