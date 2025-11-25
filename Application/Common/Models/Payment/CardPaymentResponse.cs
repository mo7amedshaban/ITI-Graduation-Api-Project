using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class CardPaymentResponse : BasePaymentResponse
    {
        [JsonProperty("data")] public CardPaymentResponseDataModel Data { get; set; }

        public class CardPaymentResponseDataModel : BasePaymentDataResponse
        {
            [JsonProperty("payment_data")] public CardPaymentData PaymentData { get; set; }

            public class CardPaymentData
            {
                [JsonProperty("redirectTo")] public string RedirectTo { get; set; }
            }
        }
    }
}
