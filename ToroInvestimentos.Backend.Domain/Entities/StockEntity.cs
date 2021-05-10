namespace ToroInvestimentos.Backend.Domain.Entities
{
    public class StockEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Variation { get; set; }
    }
}