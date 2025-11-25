using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class MeezaPaymentResponse : BasePaymentResponse
    {
        [JsonProperty("data")]
        public MeezaPaymentResponseDataModel Data { get; set; }

        public class MeezaPaymentResponseDataModel : BasePaymentDataResponse
        {
            [JsonProperty("payment_data")]
            public MeezaPaymentData PaymentData { get; set; }

            public class MeezaPaymentData
            {
                [JsonProperty("meezaReference")]
                public string MeezaReference { get; set; }

                [JsonProperty("meezaQrCode")]
                public string MeezaQrCode { get; set; }
            }
        }
    }
}
