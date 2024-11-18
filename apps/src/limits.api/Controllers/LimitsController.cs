using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Limits.API.Model;

namespace Limits.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}")]
    [ApiController]
    public class LimitsController : ControllerBase
    {

        private readonly Config config;

        public LimitsController(Config config)
        {
            this.config = config;
        }
 
        [HttpGet("limits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLimit([FromQuery] string cif)
        {
            return Ok(new
            {
                DailyLimit = 100,
                MonthlyLimit = 1000,
                WeeklyLimit = 5000,
                Region = config.Region
            });
        }

        [HttpPost("limits")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] UpdateLimitRequest request)
        {
            return Ok();
        }
    }
}
