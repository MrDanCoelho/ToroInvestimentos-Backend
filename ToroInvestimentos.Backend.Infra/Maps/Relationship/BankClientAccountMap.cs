using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;

namespace ToroInvestimentos.Backend.Infra.Maps.Relationship
{
    public class BankClientAccountMap : DommelEntityMap<BankClientAccountEntity>
    {
        public BankClientAccountMap()
        {
            ToTable("BankClientAccount");
            Map(p => p.Id).IsKey();
        }
    }
}