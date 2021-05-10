using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.Application.Services;
using ToroInvestimentos.Backend.Domain.Entities.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Services
{
    public class CrudServiceTest
    {
        private readonly Mock<ILogger<CrudService<UserEntity>>> _logger;
        private readonly Mock<ICrudRepository<UserEntity>> _crudRepository;
        
        public CrudServiceTest()
        {
            _logger = new Mock<ILogger<CrudService<UserEntity>>>();
            _crudRepository = new Mock<ICrudRepository<UserEntity>>();
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            _crudRepository.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<UserEntity>>(new List<UserEntity>()));
            
            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await service.GetAll();
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudRepository.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new UserEntity()));
            
            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await service.GetById(1);
            
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Select()
        {
            // Arrange
            _crudRepository.Setup(x => x.Select(a => a.Id == 1))
                .Returns(Task.FromResult<IEnumerable<UserEntity>>(new List<UserEntity>()));
            
            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await service.Select(a => a.Id == 1);
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var obj = new UserEntity();
            
            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.Insert(obj));
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task InsertAll()
        {
            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.InsertAll(new List<UserEntity>()));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var obj = new UserEntity();

            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Update(obj));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var obj = new UserEntity();

            // Act
            var service = new CrudService<UserEntity>(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Delete(obj));
            
            // Assert
            Assert.Null(result);
        }
    }
}