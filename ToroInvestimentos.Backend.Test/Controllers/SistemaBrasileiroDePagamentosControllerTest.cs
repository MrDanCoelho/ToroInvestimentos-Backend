using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.API.Controllers.v1;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Controllers
{
    public class SistemaBrasileiroDePagamentosControllerTest
    {
        private readonly Mock<ILogger<SistemaBrasileiroDePagamentosController>> _logger;
        private readonly Mock<IBankService> _bankService;

        public SistemaBrasileiroDePagamentosControllerTest()
        {
            _logger = new Mock<ILogger<SistemaBrasileiroDePagamentosController>>();
            _bankService = new Mock<IBankService>();
        }
        
        [Fact]
        public async Task Receive()
        {
            // Arrange
            var obj = new SbpEventDto();
            
            // Act
            var controller = new SistemaBrasileiroDePagamentosController(_logger.Object, _bankService.Object);
            var result = await controller.Receive(obj);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}