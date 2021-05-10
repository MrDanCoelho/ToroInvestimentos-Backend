using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;

namespace ToroInvestimentos.Backend.Domain.Entities.Relationship
{
    public class BankClientAccountEntity
    {
        public int Id { get; set; }
        public int BankClientId { get; set; }
        public int BankAccountId { get; set; }
        
        public BankClientEntity BankClient { get; set; }
        public BankAccountEntity BankAccount { get; set; }
    }
}