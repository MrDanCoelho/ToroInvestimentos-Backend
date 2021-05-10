using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities;

namespace ToroInvestimentos.Backend.Infra.Maps
{
    public class RefreshTokenMap: DommelEntityMap<RefreshTokenEntity>
    {
        public RefreshTokenMap()
        {
            ToTable("RefreshToken");
            Map(p => p.Id).IsKey();
        }
    }
}