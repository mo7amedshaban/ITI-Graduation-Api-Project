using Application.Common.Interfaces;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers

{
    [Route("api/FawaterakWebhooks")]
    [ApiController]
    public class FawaterakWebhooksController : ControllerBase
    {

        private readonly IFawaterakPaymentService _payments;

        public FawaterakWebhooksController(IFawaterakPaymentService payments)
        {
            _payments = payments;
        }

  
        [HttpPost("paid_json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> WebhookPaid([FromBody] WebHookModel model)
        {
            var valid = _payments.VerifyWebhook(model);
            if (!valid) return Unauthorized();

            // Handle the payment logic here

            return Ok("got it!");
        }

     
        [HttpPost("cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult WebhookCancel([FromBody] CancelTransactionModel model)
        {
            var valid = _payments.VerifyCancelTransaction(model);
            if (!valid) return Unauthorized();

            // Handle the cancellation logic here

            return Ok();
        }

       
        [HttpPost("failed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult WebhookFaild([FromBody] CancelTransactionModel model)
        {
            var valid = _payments.VerifyCancelTransaction(model);
            if (!valid) return Unauthorized();

            // Handle the failed logic here

            return Ok();
        }
    }
}
