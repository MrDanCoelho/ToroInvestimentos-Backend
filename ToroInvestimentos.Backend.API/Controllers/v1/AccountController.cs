using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToroInvestimentos.Backend.Domain.Dto;
using ToroInvestimentos.Backend.Domain.Interfaces.IServices;

// ReSharper disable RouteTemplates.MethodMissingRouteParameters
// ReSharper disable RouteTemplates.ControllerRouteParameterIsNotPassedToMethods

namespace ToroInvestimentos.Backend.API.Controllers.v1
{
    /// <summary>
    /// Controller for Account operations
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        /// <summary>
        /// Controller for Account operations
        /// </summary>
        /// <param name="logger">The app's logger</param>
        /// <param name="accountService">Service with account operations</param>
        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
            )
        {
            _logger = logger;
            _accountService = accountService;
        }
        
        /// <summary>
        /// Authenticates user in the API
        /// </summary>
        /// <param name="user">The user to authenticate</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Account/Login
        ///     {
        ///         "identity": "admin",
        ///         "password": "password",
        ///         "ipAddress": ""
        ///     }
        ///
        /// </remarks>
        /// <returns>The authenticated user</returns>
        /// <response code="200">Request was successful and user was authenticated</response>
        /// <response code="401">Request was successful but no user was returned</response>
        [HttpPost("Login")]
        public async Task<ActionResult<AuthUserResponseDto>> Login([Required] AuthUserDto user)
        {
            try
            {
                var authUser = await _accountService.Authenticate(user);
                    
                return Ok(authUser);
            }
            catch (AuthenticationException e)
            {
                _logger.LogError(e, "Wrong username and/or password");
                
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to authenticate user due to an unknown error");
                
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Unable to authenticate user. If problem persists, contact an administrator");
            }
        }
    }
}