using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.API.Controllers.v1;
using ToroInvestimentos.Backend.Domain.Entities.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Controllers
{
    public class CrudControllerBaseTest
    {
        private readonly Mock<ILogger<CrudControllerBase<UserEntity>>> _logger;
        private readonly Mock<ICrudService<UserEntity>> _crudService;


        public CrudControllerBaseTest()
        {
            _logger = new Mock<ILogger<CrudControllerBase<UserEntity>>>();
            _crudService = new Mock<ICrudService<UserEntity>>();
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var objList = new UserEntity[]
            {
                new UserEntity()
            };
            _crudService.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<UserEntity>>(objList));
            
            // Act
            var controller = new CrudControllerBase<UserEntity>(_logger.Object, _crudService.Object);
            var result = await controller.GetAll();
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudService.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new UserEntity()));
            
            // Act
            var controller = new CrudControllerBase<UserEntity>(_logger.Object, _crudService.Object);
            var result = await controller.GetById(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var log = new UserEntity();
            
            // Act
            var controller = new CrudControllerBase<UserEntity>(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(async () => await controller.Insert(log));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var log = new UserEntity();

            // Act
            var controller = new CrudControllerBase<UserEntity>(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Update(log));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Act
            var controller = new CrudControllerBase<UserEntity>(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Delete(1));
            
            // Assert
            Assert.Null(result);
        }
    }
}