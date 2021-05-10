using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Enums;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.Relationship;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

namespace ToroInvestimentos.Backend.Application.Services
{
    /// <inheritdoc/>
    public class BankService : IBankService
    {
        private readonly ILogger<BankService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IBankClientRepository _bankClientRepository;
        private readonly IBankClientAccountRepository _bankClientAccountRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountExchangeRepository _bankAccountExchangeRepository;
        private readonly IBankAccountStockRepository _bankAccountStockRepository;
        private readonly IUserBankClientRepository _userBankClientRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appSettings"></param>
        /// <param name="mapper"></param>
        /// <param name="bankClientRepository"></param>
        /// <param name="bankClientAccountRepository"></param>
        /// <param name="bankAccountRepository"></param>
        /// <param name="bankAccountExchangeRepository"></param>
        /// <param name="bankAccountStockRepository"></param>
        /// <param name="userBankClientRepository"></param>
        public BankService(ILogger<BankService> logger, AppSettings appSettings, IMapper mapper,
            IBankClientRepository bankClientRepository,
            IBankClientAccountRepository bankClientAccountRepository,
            IBankAccountRepository bankAccountRepository,
            IBankAccountExchangeRepository bankAccountExchangeRepository,
            IBankAccountStockRepository bankAccountStockRepository,
            IUserBankClientRepository userBankClientRepository)
        {
            _logger = logger;
            _appSettings = appSettings;
            _mapper = mapper;
            _bankClientRepository = bankClientRepository;
            _bankClientAccountRepository = bankClientAccountRepository;
            _bankAccountRepository = bankAccountRepository;
            _bankAccountExchangeRepository = bankAccountExchangeRepository;
            _bankAccountStockRepository = bankAccountStockRepository;
            _userBankClientRepository = userBankClientRepository;
        }

        public async Task<IEnumerable<BankClientEntity>> GetBankClients(int userId)
        {
            try
            {
                var userBankClientList = await _userBankClientRepository.Select(a => a.UserId == userId);
                var bankClientIdList = userBankClientList.Select(a => a.BankClientId).ToList();

                var result = await _bankClientRepository.Select(a => bankClientIdList.Contains(a.Id));

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get bank clients for user ID: {@UserId}", userId);
                throw;
            }
        }
        
        public async Task<IEnumerable<BankAccountEntity>> GetBankAccount(int clientId)
        {
            try
            {
                var bankClientAccountList = await _bankClientAccountRepository.Select(a => a.BankClientId == clientId);
                var bankAccountIdList = bankClientAccountList.Select(a => a.BankAccountId).ToList();

                var result = await _bankAccountRepository.Select(a => bankAccountIdList.Contains(a.Id));

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get bank accounts for client ID: {@ClientId}", clientId);
                throw;
            }
        }
        
        public async Task<IEnumerable<BankAccountStockEntity>> GetBankAccountStocks(int bankAccountId)
        {
            try
            {
                var result = await _bankAccountStockRepository.Select(a => a.BankAccountId == bankAccountId);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get bank account stocks for account ID: {@BankAccountId}", bankAccountId);
                throw;
            }
        }
        
        public async Task ReceiveEvent(SbpEventDto sbpEventDto)
        {
            try
            {
                if (sbpEventDto.Target.Bank != _appSettings.ToroBankCode)
                {
                    _logger.LogInformation("SBP event not targeted at Toro {@SbpEventDto}", sbpEventDto);
                    return;
                }

                if (sbpEventDto.Target.Branch != _appSettings.ToroBranch)
                {
                    _logger.LogWarning("SBP event not targeted at Toro's branch: {@SbpEventDto}", sbpEventDto);
                    return;
                }

                var client = (await _bankClientRepository.Select(a => a.Document == sbpEventDto.Origin.Cpf)).FirstOrDefault();

                if (client == null)
                {
                    _logger.LogWarning("Client does not exist in database: {@Transfer}", sbpEventDto);
                    return;
                }

                var clientAccount = (await _bankClientAccountRepository.Select(a => a.BankClientId == client.Id)).FirstOrDefault();

                if (clientAccount == null)
                {
                    _logger.LogWarning("Client account does not exist in database: {@Transfer}", sbpEventDto);
                    return;
                }

                var bankAccount = (await _bankAccountRepository.Select(a =>
                        a.Id == clientAccount.BankAccountId && a.AccountNumber == sbpEventDto.Target.Account
                        && a.Branch == sbpEventDto.Target.Branch)).FirstOrDefault();
                
                if (bankAccount == null)
                {
                    _logger.LogWarning("SBP event does not match any client account number and/or branch: {@Transfer}", sbpEventDto);
                    return;
                }
                
                if(!Enum.TryParse(sbpEventDto.Event, true, out FlagSbpEvent flagSbpEvent))
                    throw new NotImplementedException($"Sbp event not implemented: {sbpEventDto.Event}");

                switch (flagSbpEvent)
                {
                    case FlagSbpEvent.Transfer:
                        await ProcessTransfer(bankAccount, sbpEventDto); break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid SBP event: {flagSbpEvent}");
                }

                _logger.LogInformation("Bank transfer successful: {@Transfer}", sbpEventDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure processing bank transfer: {@Transfer}", sbpEventDto);
                throw;
            }
        }

        private async Task ProcessTransfer(BankAccountEntity account, SbpEventDto sbpEvent)
        {
            account.BalanceInBrl += sbpEvent.Amount;

            await _bankAccountRepository.Update(account);
            
            // var bankClientAccountExchange = _mapper.Map<SbpEventDto, BankAccountExchangeEntity>(sbpEvent);
            // bankClientAccountExchange.BankAccountId = account.Id;
            //
            // await _bankAccountExchangeRepository.Insert(bankClientAccountExchange);

            // _bankAccountExchangeRepository.Save();
            _bankAccountRepository.Save();
        }
    }
}