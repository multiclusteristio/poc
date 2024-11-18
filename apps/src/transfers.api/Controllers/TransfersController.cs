using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quantic.Core;
using Quantic.Web;
using Transfers.API.Commands;
using Transfers.API.Model;
using Transfers.API.Query;

namespace Transfers.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}")]
    [ApiController]
    public class TransfersController : BaseController
    {
        private readonly ICommandHandler<DoTransfer> doTransferHandler;
        private readonly IQueryHandler<GetAccountByNumber, Account> getAccountByNumberHandler;
        private readonly IQueryHandler<GetCustomerLimit, Limit> getCustomerLimitHandler;
        private readonly Config config;

        public TransfersController(ICommandHandler<DoTransfer> doTransferHandler,
            IQueryHandler<GetAccountByNumber, Account> getAccountByNumberHandler,
            IQueryHandler<GetCustomerLimit, Limit> getCustomerLimitHandler,
            Config config)
        {
            this.doTransferHandler = doTransferHandler;
            this.getAccountByNumberHandler = getAccountByNumberHandler;
            this.getCustomerLimitHandler = getCustomerLimitHandler;
            this.config = config;
        }

        [HttpPost("transfers")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error[]), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoTransfer([FromBody] DoTransferRequest request)
        {
            var doTransfer = await doTransferHandler.Handle(new DoTransfer(request.Sender, request.Receiver, request.Amount, request.Currency), Context);
            return doTransfer.Ok();
        }
    }
}
