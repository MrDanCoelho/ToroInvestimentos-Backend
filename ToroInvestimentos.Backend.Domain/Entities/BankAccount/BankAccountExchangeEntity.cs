using System;

namespace ToroInvestimentos.Backend.Domain.Entities.BankAccount
{
    public class BankAccountExchangeEntity
    {
        public int Id { get; set; }

        public int BankAccountId { get; set; }
        
        public string OriginBankCode { get; set; }

        public string OriginBankBranch { get; set; }

        public string OriginBankAccountNumber { get; set; }

        public decimal Value { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}