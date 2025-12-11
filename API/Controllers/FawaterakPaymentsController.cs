using Application.Common.Interfaces;
using Application.DTOs.Payment;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/FawaterakPayments")]
    [ApiController]
    public class FawaterakPaymentsController : ControllerBase
    {


        private readonly IFawaterakPaymentService _payments;

        public FawaterakPaymentsController(IFawaterakPaymentService payments)
        {
            _payments = payments;
        }


        [HttpPost("invoices")]
        [ProducesResponseType(typeof(EInvoiceResponseModel.EInvoiceResponseDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EInvoiceResponseModel.EInvoiceResponseDataModel>>
            CreateInvoice([FromBody] EInvoiceRequestModel request)
        {
            var data = await _payments.CreateEInvoiceAsync(request);
            if (data is null) throw new Exception("Error in creating invoice");
            return Ok(data);

        }

        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(IList<Core.DTOs.PaymentMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IList<Core.DTOs.PaymentMethod>>> GetPaymentMethods()
        {
            var result = await _payments.GetPaymentMethods();
            if (result is null || result.Count == 0) return NoContent();
            return Ok(result);
        }



        //[HttpPost("pay")]
        //[ProducesResponseType(typeof(BasePaymentDataResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<BasePaymentDataResponse>> Pay([FromBody] EInvoiceRequestModel invoice)
        //{
        //    var result = await _payments.GeneralPay(invoice);
        //    if (result is null) return BadRequest();
        //    return Ok(result);
        //}


        //[HttpGet("iframe-hash")]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //public ActionResult<string> IFrameHash([FromQuery] string domain)
        //{
        //    var result = _payments.GenerateHashKeyForIFrame(domain);
        //    return Ok(result);
        //}
    }
}
