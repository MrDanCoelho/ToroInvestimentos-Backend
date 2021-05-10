using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Entities.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IAdapters;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories;
using ToroInvestimentos.Backend.Domain.Interfaces.IRepositories.BankAccount;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

namespace ToroInvestimentos.Backend.Application.Services
{
    public class BrokerService : IBrokerService
    {
        private readonly ILogger<BrokerService> _logger;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IBankAccountStockRepository _bankAccountStockRepository;
        private readonly IBrokerAdapter _brokerAdapter;
        private readonly IStockRepository _stockRepository;

        public BrokerService(ILogger<BrokerService> logger,
            IBankAccountRepository bankAccountRepository,
            IBankAccountStockRepository bankAccountStockRepository,
            IBrokerAdapter brokerAdapter, 
            IStockRepository stockRepository)
        {
            _logger = logger;
            _bankAccountRepository = bankAccountRepository;
            _bankAccountStockRepository = bankAccountStockRepository;
            _brokerAdapter = brokerAdapter;
            _stockRepository = stockRepository;
        }

        public async Task<List<StockEntity>> GetTrending()
        {
            try
            {
                var stocks = await _stockRepository.GetAll();

                var topStocks = stocks.OrderByDescending(a => a.Variation).Take(5).ToList();

                return topStocks;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Service was unable to get 5 most traded stocks");
                throw;
            }
        }
        
        public async Task ProcessOrder(BankAccountEntity bankAccount, BrokerOrderDto order)
        {
            try
            {
                var stockList = (await _stockRepository.Select(a => a.Symbol == order.Symbol)).ToList();

                if (stockList.Count() > 1)
                    throw new ArgumentException($"Symbol {order.Symbol.ToUpper()} is not unique");

                var stock = stockList.Single();

                var stockValue = stock.CurrentPrice * order.Amount;

                bankAccount.BalanceInBrl -= stockValue;
                
                if (bankAccount.BalanceInBrl < 0)
                    throw new ArgumentException("Insufficient balance in account");
                
                await _bankAccountRepository.Update(bankAccount);

                var accountStock = (await _bankAccountStockRepository.Select(a =>
                    a.BankAccountId == bankAccount.Id && a.Symbol == order.Symbol)).SingleOrDefault();
                if (accountStock != null)
                {
                    accountStock.Amount += order.Amount;
                    if(accountStock.Amount < 0)
                        throw new ArgumentException("Insufficient stocks to sell");

                    await _bankAccountStockRepository.Update(accountStock);
                }
                else
                {
                    if(order.Amount < 0)
                        throw new ArgumentException("Insufficient stocks to sell");
                    
                    var newAccountStock = new BankAccountStockEntity()
                    {
                        BankAccountId = bankAccount.Id,
                        Symbol = stock.Symbol,
                        Amount = order.Amount
                    };

                    await _bankAccountStockRepository.Insert(newAccountStock);
                }

                _bankAccountStockRepository.Save();
                _bankAccountRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Service could not complete order");
                throw;
            }
        }

        public async Task<IEnumerable<StockEntity>> GetStocks(List<string> stockIdList)
        {
            try
            {
                var stocks = await _stockRepository.Select(a => stockIdList.Contains(a.Symbol));

                return stocks;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Service was unable to get stocks");
                throw;
            }
        }
    }
}