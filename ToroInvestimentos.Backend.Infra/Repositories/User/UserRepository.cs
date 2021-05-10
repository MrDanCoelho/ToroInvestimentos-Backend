using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Entities.User;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.User;

namespace ToroInvestimentos.Backend.Infra.Repositories.User
{
    public class UserRepository : CrudRepository<UserEntity>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        public UserRepository(
            ILogger<UserRepository> logger, 
            IUnitOfWork unitOfWork
            ) : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}