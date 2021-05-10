namespace ToroInvestimentos.Backend.Domain.Entities.BankAccount
{
    public class BankAccountStockEntity
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public int Amount { get; set; }
        public string Symbol { get; set; }
    }
}