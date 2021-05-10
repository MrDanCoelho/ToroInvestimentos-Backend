namespace ToroInvestimentos.Backend.Domain.Dto.BrokerDto
{
    public class QuoteDto
    {
        public string NumeroConta { get; set; }
        public string Quantidade { get; set; }
        public int ValorTotal { get; set; }
        public string Language { get; set; }
        public string Region { get; set; }
        public string QuoteType { get; set; }
        public bool Triggerable { get; set; }
        public string QuoteSourceName { get; set; }
        public string ShortName { get; set; }
        public string Exchange { get; set; }
        public string Market { get; set; }
        public string MarketState { get; set; }
        public double RegularMarketPrice { get; set; }
        public int RegularMarketTime { get; set; }
        public double RegularMarketChange { get; set; }
        public double PostMarketChangePercent { get; set; }
        public int PostMarketTime { get; set; }
        public int SourceInterval { get; set; }
        public string ExchangeTimezoneName { get; set; }
        public string ExchangeTimezoneShortName { get; set; }
        public int GmtOffSetMilliseconds { get; set; }
        public bool EsgPopulated { get; set; }
        public bool Tradeable { get; set; }
        public int ExchangeDataDelayedBy { get; set; }
        public int PriceHint { get; set; }
        public double PostMarketPrice { get; set; }
        public double PostMarketChange { get; set; }
        public double RegularMarketChangePercent { get; set; }
        public double RegularMarketPreviousClose { get; set; }
        public string FullExchangeName { get; set; }
        public string Symbol { get; set; }
    }
}