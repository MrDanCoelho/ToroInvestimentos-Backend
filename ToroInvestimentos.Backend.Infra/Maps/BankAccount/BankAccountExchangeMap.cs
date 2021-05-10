using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;

namespace ToroInvestimentos.Backend.Infra.Maps.BankAccount
{
    public class BankAccountExchangeMap : DommelEntityMap<BankAccountExchangeEntity>
    {
        public BankAccountExchangeMap()
        {
            ToTable("BankAccountExchange");
            Map(p => p.Id).IsKey();
        }
    }
}