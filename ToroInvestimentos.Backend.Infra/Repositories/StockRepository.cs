using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;

namespace ToroInvestimentos.Backend.Infra.Repositories
{
    public class StockRepository : CrudRepository<StockEntity>, IStockRepository
    {
        private readonly ILogger<StockRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public StockRepository(ILogger<StockRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}