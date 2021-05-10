using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.API.Controllers.v1;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Controllers
{
    public class BankControllerTest
    {
        private readonly Mock<ILogger<BankController>> _logger;
        private readonly Mock<IBankService> _bankService;
        private readonly Mock<IBrokerService> _brokerService;
        private readonly Mock<IHttpContextAccessor> _context;

        public BankControllerTest()
        {
            _logger = new Mock<ILogger<BankController>>();
            _bankService = new Mock<IBankService>();
            _brokerService = new Mock<IBrokerService>();
            _context = new Mock<IHttpContextAccessor>();
        }
        
        [Fact]
        public async Task GetUserPosition()
        {
            // Arrange
            _context.Setup(x => x.HttpContext.User.Identity.Name).Returns("0");
            _bankService.Setup(x => x.GetBankClients(0)).ReturnsAsync(new []{ new BankClientEntity{ Id = 0 }});
            _bankService.Setup(x => x.GetBankAccount(0)).ReturnsAsync(new [] { new BankAccountEntity{ Id=0 } });
            _bankService.Setup(x => x.GetBankAccountStocks(0)).ReturnsAsync(new [] { new BankAccountStockEntity { Symbol = "a"} });
            _brokerService.Setup(x => x.GetStocks(new List<string>() {"a"})).ReturnsAsync(new StockEntity[] { });
            
            // Act
            var controller = new BankController(_logger.Object, _bankService.Object, _brokerService.Object, _context.Object);
            var result = await controller.GetUserPosition();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}