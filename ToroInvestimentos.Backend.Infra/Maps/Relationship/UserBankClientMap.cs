using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.Relationship;

namespace ToroInvestimentos.Backend.Infra.Maps.Relationship
{
    public class UserBankClientMap : DommelEntityMap<UserBankClientEntity>
    {
        public UserBankClientMap()
        {
            ToTable("UserBankClient");
            Map(p => p.Id).IsKey();
        }
    }
}