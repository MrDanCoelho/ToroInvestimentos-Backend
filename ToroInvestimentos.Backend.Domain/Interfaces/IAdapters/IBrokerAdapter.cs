using System.Collections.Generic;
using System.Threading.Tasks;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;

namespace ToroInvestimentos.Backend.Domain.Interfaces.IAdapters
{
    public interface IBrokerAdapter
    {
        Task<List<QuoteDto>> GetRecommendation();
    }
}