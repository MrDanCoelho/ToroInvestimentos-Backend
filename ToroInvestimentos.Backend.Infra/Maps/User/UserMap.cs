using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.User;

namespace ToroInvestimentos.Backend.Infra.Maps.User
{
    public class UserMap: DommelEntityMap<UserEntity>
    {
        public UserMap()
        {
            ToTable("User");
            Map(p => p.Id).IsKey();
        }
    }
}