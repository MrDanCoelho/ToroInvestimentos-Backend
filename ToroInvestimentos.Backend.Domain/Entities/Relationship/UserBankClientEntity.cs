namespace ToroInvestimentos.Backend.Domain.Entities.Relationship
{
    public class UserBankClientEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BankClientId { get; set; }
    }
}