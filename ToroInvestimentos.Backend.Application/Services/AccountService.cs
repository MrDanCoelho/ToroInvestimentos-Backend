﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToroInvestimentos.Backend.Application.Helpers;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Dto;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

namespace ToroInvestimentos.Backend.Application.Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AccountService> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="logger"></param>
        /// <param name="refreshTokenRepository"></param>
        /// <param name="userRepository"></param>
        public AccountService(
            AppSettings appSettings,
            ILogger<AccountService> logger,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository
            )
        {
            _appSettings = appSettings;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        
        public async Task<AuthUserResponseDto> Authenticate(AuthUserDto authUserDto)
        {
            try
            {
                var userList =  await _userRepository.Select(a => a.UserName == authUserDto.Identity || a.Email == authUserDto.Identity);

                var user = userList.FirstOrDefault(a => CryptographyService.HashPassword(authUserDto.Password + a.PasswordSalt) == a.Password);

                if (user == null)
                {
                    _logger.LogInformation("User was not found. Ending process with status 401");
                    throw new AuthenticationException("Invalid user. Check your password and/or username");
                }

                _logger.LogInformation("User found");

                var jwtToken = GenerateJwtToken(user);
                _logger.LogInformation("New token generated");

                var refreshToken = GenerateRefreshToken(authUserDto.IpAddress);
                refreshToken.UserId = user.Id;

                await _refreshTokenRepository.Insert(refreshToken);
                _refreshTokenRepository.Save();
                
                _logger.LogInformation("New refresh token generated");

                var authUserResponse = new AuthUserResponseDto()
                {
                    Username = user.UserName,
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken.Token
                };

                _logger.LogInformation("Authentication successful");
                
                return authUserResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during authentication");
                throw;
            }
        }
        
        private string GenerateJwtToken(UserEntity user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] 
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to generate JWT token for user={@User}", user);
                throw;
            }
        }

        private RefreshTokenEntity GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}