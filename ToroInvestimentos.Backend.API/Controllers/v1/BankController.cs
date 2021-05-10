using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Dto;
using ToroInvestimentos.Backend.Domain.Entities.BankClient;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

// ReSharper disable RouteTemplates.MethodMissingRouteParameters
// ReSharper disable RouteTemplates.ControllerRouteParameterIsNotPassedToMethods

namespace ToroInvestimentos.Backend.API.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;
        private readonly IBankService _bankService;
        private readonly IBrokerService _brokerService;
        private readonly IHttpContextAccessor _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="bankService"></param>
        /// <param name="brokerService"></param>
        /// <param name="context"></param>
        public BankController(ILogger<BankController> logger, IBankService bankService, IBrokerService brokerService, IHttpContextAccessor context)
        {
            _logger = logger;
            _bankService = bankService;
            _brokerService = brokerService;
            _context = context;
        }
        
        /// <summary>
        /// Get User Account position
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Bank/GetUserPosition
        ///
        /// </remarks>
        /// <returns>Status code of the operation</returns>
        /// <response code="200">Transfer received with success</response>
        [HttpGet("GetUserPosition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BankClientEntity>>> GetUserPosition()
        {
            try
            {
                var user = Convert.ToInt32(_context.HttpContext.User.Identity.Name);
                
                var clientList = await _bankService.GetBankClients(user);
                var client = clientList.First();

                var clientAccountList = await _bankService.GetBankAccount(client.Id);
                var clientAccount = clientAccountList.First();

                var clientStocks = (await _bankService.GetBankAccountStocks(clientAccount.Id)).Where(a => a.Amount > 0)
                    .ToList();
                var stockInfo = await _brokerService.GetStocks(clientStocks.Select(a => a.Symbol).ToList());

                var stocks = stockInfo.Select(a => new StockDto()
                {
                    Symbol = a.Symbol,
                    Name = a.Name,
                    Quantity = clientStocks.Single(b => b.Symbol == a.Symbol).Amount,
                    CurrentPrice = a.CurrentPrice,
                    Variation = a.Variation
                }).ToList();

                var result = new UserPositionDto()
                {
                    Client = client,
                    Account = clientAccount,
                    Stocks = stocks
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "The server was unable to process bank transfer. If problem persists, contact an administrator");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred and the server could not receive the bank transfer. If the problem persists, contact an administrator");
            }
        }
    }
}