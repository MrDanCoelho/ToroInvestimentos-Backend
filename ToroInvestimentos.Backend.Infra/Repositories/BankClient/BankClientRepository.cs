using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankClient;

namespace ToroInvestimentos.Backend.Infra.Repositories.BankClient
{
    /// <inheritdoc cref="IBankClientRepository" />
    public class BankClientRepository : CrudRepository<BankClientEntity>, IBankClientRepository
    {
        private readonly ILogger<BankClientRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public BankClientRepository(ILogger<BankClientRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}