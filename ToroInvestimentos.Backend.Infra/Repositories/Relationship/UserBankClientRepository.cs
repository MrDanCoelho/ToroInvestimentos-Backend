using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.Relationship;

namespace ToroInvestimentos.Backend.Infra.Repositories.Relationship
{
    /// <inheritdoc cref="IUserBankClientRepository"/>
    public class UserBankClientRepository : CrudRepository<UserBankClientEntity>, IUserBankClientRepository
    {
        private readonly ILogger<UserBankClientRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public UserBankClientRepository(
            ILogger<UserBankClientRepository> logger, 
            IUnitOfWork unitOfWork
        ) : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}