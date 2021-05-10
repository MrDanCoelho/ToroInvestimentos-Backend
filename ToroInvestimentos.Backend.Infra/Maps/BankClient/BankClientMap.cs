using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;

namespace ToroInvestimentos.Backend.Infra.Maps.BankClient
{
    public class BankClientMap : DommelEntityMap<BankClientEntity>
    {
        public BankClientMap()
        {
            ToTable("BankClient");
            Map(p => p.Id).IsKey();
        }
    }
}