using System.Collections.Generic;

namespace ToroInvestimentos.Backend.Domain.Entities.BankAccount
{
    public class BankAccountEntity
    {
        public int Id { get; set; }
        
        public string BankCode { get; set; }

        public string Branch { get; set; }
        
        public string AccountNumber { get; set; }

        public decimal BalanceInBrl { get; set; }
    }
}