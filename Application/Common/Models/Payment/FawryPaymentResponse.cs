using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class FawryPaymentResponse : BasePaymentResponse
    {
        [JsonProperty("data")]
        public FawryPaymentResponseDataModel Data { get; set; }

        public class FawryPaymentResponseDataModel : BasePaymentDataResponse
        {
            [JsonProperty("payment_data")]
            public FawryPaymentData PaymentData { get; set; }

            public class FawryPaymentData
            {
                [JsonProperty("fawryCode")]
                public string FawryCode { get; set; }
                [JsonProperty("expireDate")]
                public DateTime ExpireDate { get; set; }
            }
        }
    }
}