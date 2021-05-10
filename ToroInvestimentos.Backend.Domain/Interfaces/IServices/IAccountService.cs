using System.Threading.Tasks;
using ToroInvestimentos.Backend.Domain.Dto;

namespace ToroInvestimentos.Backend.Domain.Interfaces.IServices
{
    /// <summary>
    /// Service with account methods
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="authUserDto">DTO with user information</param>
        /// <returns>Authenticated user</returns>
        Task<AuthUserResponseDto> Authenticate(AuthUserDto authUserDto);
    }
}