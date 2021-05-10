using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.Relationship;

namespace ToroInvestimentos.Backend.Infra.Repositories.Relationship
{
    /// <inheritdoc cref="IBankClientAccountRepository"/>
    public class BankClientAccountRepository : CrudRepository<BankClientAccountEntity>, IBankClientAccountRepository
    {
        private readonly ILogger<BankClientAccountRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public BankClientAccountRepository(ILogger<BankClientAccountRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}