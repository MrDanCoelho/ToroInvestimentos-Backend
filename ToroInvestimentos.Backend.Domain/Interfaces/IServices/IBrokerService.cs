using System.Collections.Generic;
using System.Threading.Tasks;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;

namespace ToroInvestimentos.Backend.Domain.Interfaces.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBrokerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<StockEntity>> GetTrending();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task ProcessOrder(BankAccountEntity bankAccount, BrokerOrderDto order);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockIdList"></param>
        /// <returns></returns>
        Task<IEnumerable<StockEntity>> GetStocks(List<string> stockIdList);
    }
}