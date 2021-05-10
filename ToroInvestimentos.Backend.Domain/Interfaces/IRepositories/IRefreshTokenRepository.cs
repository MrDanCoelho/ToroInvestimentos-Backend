using ToroInvestimentos.Backend.Domain.Entities;

namespace ToroInvestimentos.Backend.Domain.Interfaces.IRepositories
{
    /// <summary>
    /// Repository with methods for token refreshing
    /// </summary>
    public interface IRefreshTokenRepository : ICrudRepository<RefreshTokenEntity>
    {
        
    }
}