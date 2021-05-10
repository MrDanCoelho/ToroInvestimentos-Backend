using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;

namespace ToroInvestimentos.Backend.Infra.Maps.BankAccount
{
    public class BankAccountMap : DommelEntityMap<BankAccountEntity>
    {
        public BankAccountMap()
        {
            ToTable("BankAccount");
            Map(p => p.Id).IsKey();
        }
    }
}