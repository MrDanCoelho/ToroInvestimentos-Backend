using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.API.Controllers.v1;
using ToroInvestimentos.Backend.Domain.Dto;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Controllers
{
    public class AccountControllerTest
    {
        private readonly Mock<ILogger<AccountController>> _logger;
        private readonly Mock<IAccountService> _accountService;

        public AccountControllerTest()
        {
            _logger = new Mock<ILogger<AccountController>>();
            _accountService = new Mock<IAccountService>();
        }

        [Fact]
        public async Task Login()
        {
            // Arrange
            var authUserDto = new AuthUserDto();
            
            // Act
            var controller = new AccountController(_logger.Object, _accountService.Object);
            var result = await controller.Login(authUserDto);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}