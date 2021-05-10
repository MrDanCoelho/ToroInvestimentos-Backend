using System.Collections.Generic;

namespace ToroInvestimentos.Backend.Domain.Dto.BrokerDto
{
    public class BrokerResultDto
    {
        public int Count { get; set; }
        public List<QuoteDto> Quotes { get; set; }
    }
}