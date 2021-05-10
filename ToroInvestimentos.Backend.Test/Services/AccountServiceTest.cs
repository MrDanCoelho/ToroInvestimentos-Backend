﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ToroInvestimentos.Backend.Application.Helpers;
using ToroInvestimentos.Backend.Application.Services;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Dto;
using ToroInvestimentos.Backend.Domain.Entities.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.User;
using Xunit;

namespace ToroInvestimentos.Backend.Test.Services
{
    public class AccountServiceTest
    {
        private readonly AppSettings _appSettings;
        private readonly Mock<ILogger<AccountService>> _logger;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Mock<IUserRepository> _userRepository;
        
        public AccountServiceTest()
        {
            _appSettings = new AppSettings();
            _logger = new Mock<ILogger<AccountService>>();
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _userRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Authenticate()
        {
            // Arrange
            _appSettings.Secret = "atleast32bitsizedsecretstring";
            var authUserDto = new AuthUserDto()
            {
                Password = ""
            };
            var password = CryptographyService.HashPassword("");
            _userRepository.Setup(x => x.Select(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .ReturnsAsync(new List<UserEntity> { new UserEntity { UserName = "a", Password = password, PasswordSalt = "" } });
            
            // Act
            var service = new AccountService(_appSettings, _logger.Object, _refreshTokenRepository.Object,
                _userRepository.Object);
            var result = await service.Authenticate(authUserDto);
            
            // Assert
            Assert.NotNull(result);
        }
    }
}