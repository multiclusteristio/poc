using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Accounts.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // Version 1.0 - Get Account
        [HttpGet("v1/accounts")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccount([FromQuery] string accountNumber)
        {
            return Ok(new Account
            {
                AccountNumber = accountNumber,
                AccountType = "Loan V1",
                CIF = "1",
                Balance = 10,
                BranchCode = "9142"
            });
        }

        // Version 1.0 - Create Account
        [HttpPost("v1/accounts")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewAccountRequest request)
        {
            return Created($"accounts/{Guid.NewGuid()}", null);
        }

        // Version 2.0 - Get Account
        [HttpGet("v2/accounts")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountV2([FromQuery] string accountNumber)
        {
            return Ok(new Account
            {
                AccountNumber = accountNumber,
                AccountType = "Loan V2",
                CIF = "1",
                Balance = 10,
                BranchCode = "9142"
            });
        }
    }
}
