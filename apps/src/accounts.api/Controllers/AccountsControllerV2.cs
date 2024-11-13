using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Accounts.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}")]
    [ApiController]
    public class AccountsControllerV2 : ControllerBase
    {
        [HttpGet("accounts")]
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
                AccountType = "Loan V2",
                CIF = "1",
                Balance = 10,
                BranchCode = "9142"
            });
        }
    }
}
