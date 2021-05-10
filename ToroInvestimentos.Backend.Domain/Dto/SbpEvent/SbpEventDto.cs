namespace ToroInvestimentos.Backend.Domain.Dto.SbpEvent
{
    public class SbpEventDto
    {
        public string Event { get; set; }
        public SbpEventBankDto Target { get; set; }
        public SbpEventBankDto Origin { get; set; }
        public decimal Amount { get; set; }
    }
}