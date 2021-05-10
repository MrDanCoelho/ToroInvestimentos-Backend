using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities;

namespace ToroInvestimentos.Backend.Infra.Maps
{
    public class StockMap : DommelEntityMap<StockEntity>
    {
        public StockMap()
        {
            ToTable("Stock");
            Map(p => p.Id).IsKey();
        }
    }
}