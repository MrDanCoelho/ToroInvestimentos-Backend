using System.Collections.Generic;
using System.Threading.Tasks;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;

namespace ToroInvestimentos.Backend.Domain.Interfaces.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BankClientEntity>> GetBankClients(int userId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<BankAccountEntity>> GetBankAccount(int clientId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <returns></returns>
        Task<IEnumerable<BankAccountStockEntity>> GetBankAccountStocks(int bankAccountId);
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbpEventDto"></param>
        /// <returns></returns>
        Task ReceiveEvent(SbpEventDto sbpEventDto);
    }
}