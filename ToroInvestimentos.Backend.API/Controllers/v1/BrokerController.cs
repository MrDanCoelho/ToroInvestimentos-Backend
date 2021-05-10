using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Dto.BrokerDto;
using ToroInvestimentos.Backend.Domain.Entities;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

// ReSharper disable RouteTemplates.ControllerRouteParameterIsNotPassedToMethods
// ReSharper disable RouteTemplates.MethodMissingRouteParameters

namespace ToroInvestimentos.Backend.API.Controllers.v1
{
    /// <summary>
    /// Broker operations controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BrokerController : ControllerBase
    {
        private readonly ILogger<BrokerController> _logger;
        private readonly IBankService _bankService;
        private readonly IBrokerService _brokerService;
        private readonly IHttpContextAccessor _context;

        /// <summary>
        /// Broker operations controller
        /// </summary>
        /// <param name="logger">The app's <see cref="ILogger"/></param>
        /// <param name="bankService"></param>
        /// <param name="brokerService"><see cref="IBrokerService"/> implementation</param>
        /// <param name="context"></param>
        public BrokerController(ILogger<BrokerController> logger, IBankService bankService, IBrokerService brokerService, IHttpContextAccessor context)
        {
            _logger = logger;
            _bankService = bankService;
            _brokerService = brokerService;
            _context = context;
        }

        /// <summary>
        /// Get top 5 most traded stocks from last week
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /broker/Trending
        ///
        /// </remarks>
        /// <returns>List of the top 5 quotes for the week</returns>
        /// <response code="200">Returns list of the top 5 quotes for the week</response>
        /// <response code="502">API server is currently unavailable</response>
        [HttpGet("Trending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<List<StockEntity>>> Trending()
        {
            try
            {
                var result = await _brokerService.GetTrending();

                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Broker Trending API server failure");
                return StatusCode(StatusCodes.Status502BadGateway,
                    "Failed getting a response from API. Try again later");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Broker Trending request failed with unknown error");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred and the server could not request recommended stocks. If the problem persists, contact an administrator");
            }
        }

        
        /// <summary>
        /// Process a stock order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /broker/Order
        ///     {
        ///         "symbol": "LEV",
        ///         "amount": 1
        ///     }
        /// </remarks>
        /// <returns>Status code of the operation</returns>
        /// <response code="200">Operation was completed successfully</response>
        [HttpPost("Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Order(BrokerOrderDto order)
        {
            try
            {
                var user = Convert.ToInt32(_context.HttpContext.User.Identity.Name);
                
                var clientList = await _bankService.GetBankClients(user);
                var client = clientList.First();

                var clientAccountList = await _bankService.GetBankAccount(client.Id);
                var clientAccount = clientAccountList.First();

                await _brokerService.ProcessOrder(clientAccount, order);
                
                return Ok("Order accepted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to buy stock");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred and the server could not buy the stock. If the problem persists, contact an administrator");
            }
        }
    }
}