using System.Collections.Generic;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;

namespace ToroInvestimentos.Backend.Domain.Dto
{
    public class UserPositionDto
    {
        public BankClientEntity Client { get; set; }
        public BankAccountEntity Account { get; set; }
        
        public List<StockDto> Stocks { get; set; }
    }
}