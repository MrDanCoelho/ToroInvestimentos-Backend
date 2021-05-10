using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.API.Controllers.v1;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Controllers
{
    public class BrokerControllerTest
    {
        private readonly Mock<ILogger<BrokerController>> _logger;
        private readonly Mock<IBankService> _bankService;
        private readonly Mock<IBrokerService> _brokerService;
        private readonly Mock<IHttpContextAccessor> _context;

        public BrokerControllerTest()
        {
            _logger = new Mock<ILogger<BrokerController>>();
            _bankService = new Mock<IBankService>();
            _brokerService = new Mock<IBrokerService>();
            _context = new Mock<IHttpContextAccessor>();
        }
        
        [Fact]
        public async Task Trending()
        {
            // Act
            var controller = new BrokerController(_logger.Object, _bankService.Object, _brokerService.Object, _context.Object);
            var result = await controller.Trending();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}