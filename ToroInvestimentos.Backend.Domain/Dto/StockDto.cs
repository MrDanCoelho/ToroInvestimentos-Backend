namespace ToroInvestimentos.Backend.Domain.Dto
{
    public class StockDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Variation { get; set; }
    }
}