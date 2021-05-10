using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;

namespace ToroInvestimentos.Backend.Infra.Repositories.BankAccount
{
    /// <inheritdoc cref="IBankAccountStockRepository"/>
    public class BankAccountStockRepository : CrudRepository<BankAccountStockEntity>, IBankAccountStockRepository
    {
        private readonly ILogger<BankAccountStockRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public BankAccountStockRepository(ILogger<BankAccountStockRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}