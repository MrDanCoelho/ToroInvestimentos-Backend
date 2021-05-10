using Dapper.FluentMap.Dommel.Mapping;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;

namespace ToroInvestimentos.Backend.Infra.Maps.BankAccount
{
    public class BankAccountStockMap : DommelEntityMap<BankAccountStockEntity>
    {
        public BankAccountStockMap()
        {
            ToTable("BankAccountStock");
            Map(p => p.Id).IsKey();
        }
    }
}