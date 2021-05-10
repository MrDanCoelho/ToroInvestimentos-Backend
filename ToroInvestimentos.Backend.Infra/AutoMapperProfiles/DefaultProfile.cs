using AutoMapper;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;

namespace ToroInvestimentos.Backend.Infra.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<SbpEventDto, BankAccountExchangeEntity>()
                .ForMember(a => a.Value, 
                    b => b.MapFrom(c => c.Amount))
                .ForMember(a => a.Value, 
                b => b.MapFrom(c => c.Amount));
        }
    }
}