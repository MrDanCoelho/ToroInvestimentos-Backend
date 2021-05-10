using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Dto.SbpEvent;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

// ReSharper disable RouteTemplates.ControllerRouteParameterIsNotPassedToMethods
// ReSharper disable RouteTemplates.MethodMissingRouteParameters

namespace ToroInvestimentos.Backend.API.Controllers.v1
{
    /// <summary>
    /// Sistema Brasileiro de Pagamentos operations controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SistemaBrasileiroDePagamentosController : ControllerBase
    {
        private readonly ILogger<SistemaBrasileiroDePagamentosController> _logger;
        private readonly IBankService _bankService;

        /// <summary>
        /// Sistema Brasileiro de Pagamentos operations controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="bankService"></param>
        public SistemaBrasileiroDePagamentosController(ILogger<SistemaBrasileiroDePagamentosController> logger, IBankService bankService)
        {
            _logger = logger;
            _bankService = bankService;
        }

        /// <summary>
        /// Receive and process new event from the Sistema Brasileiro de Pagamentos system
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /BankTransfer/Receive
        ///     {
        ///         "event": "TRANSFER",
        ///         "target": {
        ///             "bank": "352",
        ///             "branch": "0001",
        ///             "account": "0000000-0"
        ///         },
        ///         "origin": {
        ///             "bank": "",
        ///             "branch": "",
        ///             "account": "",
        ///             "cpf": "00000000000"
        ///         },
        ///         "amount": 1000
        ///     }
        ///
        /// </remarks>
        /// <returns>Status code of the operation</returns>
        /// <response code="200">Transfer received with success</response>
        [HttpPost("Receive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Receive([Required]SbpEventDto sbpEvent)
        {
            try
            {
                await _bankService.ReceiveEvent(sbpEvent);

                return Ok("Event received with success");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "The server was unable to process SBP event. If problem persists, contact an administrator");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred and the server could not receive the SBP event. If the problem persists, contact an administrator");
            }
        }
    }
}