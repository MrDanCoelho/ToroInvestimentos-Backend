using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;

namespace ToroInvestimentos.Backend.Infra.Repositories.BankAccount
{
    /// <inheritdoc cref="IBankAccountExchangeRepository"/>
    public class BankAccountExchangeRepository : CrudRepository<BankAccountExchangeEntity>, IBankAccountExchangeRepository
    {
        private readonly ILogger<BankAccountExchangeRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public BankAccountExchangeRepository(ILogger<BankAccountExchangeRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}