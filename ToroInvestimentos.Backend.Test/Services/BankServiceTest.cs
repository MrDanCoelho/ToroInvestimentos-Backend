using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.Application.Services;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.Relationship;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Services
{
    public class BankServiceTest
    {
        private readonly AppSettings _appSettings;
        private readonly Mock<ILogger<BankService>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBankClientRepository> _bankClientRepository;
        private readonly Mock<IBankClientAccountRepository> _bankClientAccountRepository;
        private readonly Mock<IBankAccountRepository> _bankAccountRepository;
        private readonly Mock<IBankAccountExchangeRepository> _bankAccountExchangeRepository;
        private readonly Mock<IBankAccountStockRepository> _bankAccountStockRepository;
        private readonly Mock<IUserBankClientRepository> _userBankClientRepository;
        
        public BankServiceTest()
        {
            _logger = new Mock<ILogger<BankService>>();
            _appSettings = new AppSettings();
            _mapper = new Mock<IMapper>();
            _bankClientRepository = new Mock<IBankClientRepository>();
            _bankClientAccountRepository = new Mock<IBankClientAccountRepository>();
            _bankAccountRepository = new Mock<IBankAccountRepository>();
            _bankAccountExchangeRepository = new Mock<IBankAccountExchangeRepository>();
            _bankAccountStockRepository = new Mock<IBankAccountStockRepository>();
            _userBankClientRepository = new Mock<IUserBankClientRepository>();
        }

        [Fact]
        public async Task GetBankClients()
        {
            // Arrange
            var userBankClientList = new List<UserBankClientEntity>()
            {
                new UserBankClientEntity()
                {
                    BankClientId = 0
                }
            };
            _userBankClientRepository.Setup(x => x.Select(It.IsAny<Expression<Func<UserBankClientEntity, bool>>>()))
                .ReturnsAsync(userBankClientList);
            _bankClientRepository.Setup(x => x.Select(It.IsAny<Expression<Func<BankClientEntity, bool>>>()))
                .ReturnsAsync(new BankClientEntity[] { new BankClientEntity(){ Id=0 } });
            
            // Act
            var service = new BankService(_logger.Object, _appSettings, _mapper.Object, _bankClientRepository.Object,
                _bankClientAccountRepository.Object, _bankAccountRepository.Object, _bankAccountExchangeRepository.Object, 
                _bankAccountStockRepository.Object, _userBankClientRepository.Object);
            var result = await service.GetBankClients(0);
            
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task ReceiveTransfer()
        {
            // Arrange
            var transfer = new SbpEventDto()
            {
                Target = new SbpEventBankDto()
                {
                    Bank = _appSettings.ToroBankCode,
                    Branch = _appSettings.ToroBranch,
                    Account = ""
                },
                Origin = new SbpEventBankDto()
                {
                    Cpf = ""
                }
            };
            var bankClientList = new List<BankClientEntity>()
            {
                new BankClientEntity()
            };
            var bankClientAccountList = new List<BankClientAccountEntity>()
            {
                new BankClientAccountEntity()
            };
            var bankAccountList = new List<BankAccountEntity>()
            {
                new BankAccountEntity()
            };
            _bankClientRepository.Setup(x => x.Select(It.IsAny<Expression<Func<BankClientEntity, bool>>>()))
                .ReturnsAsync(bankClientList);
            _bankClientAccountRepository.Setup(x => x.Select(It.IsAny<Expression<Func<BankClientAccountEntity, bool>>>()))
                .ReturnsAsync(bankClientAccountList);
            _bankAccountRepository.Setup(x => x.Select(It.IsAny<Expression<Func<BankAccountEntity, bool>>>()))
                .ReturnsAsync(bankAccountList);
            _mapper.Setup(x => x.Map<SbpEventDto, BankAccountExchangeEntity>(transfer))
                .Returns(new BankAccountExchangeEntity());

            // Act
            var service = new BankService(_logger.Object, _appSettings, _mapper.Object, _bankClientRepository.Object,
                _bankClientAccountRepository.Object, _bankAccountRepository.Object, _bankAccountExchangeRepository.Object, 
                _bankAccountStockRepository.Object, _userBankClientRepository.Object);
            var result = await Record.ExceptionAsync(() => service.ReceiveEvent(transfer));
            
            // Assert
            Assert.NotNull(result);
        }
    }
}