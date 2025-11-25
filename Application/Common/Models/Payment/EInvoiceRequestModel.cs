using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
 
    public class EInvoiceResponseModel
    {
      
        [JsonProperty("status")]
        public string Status { get; set; }

      
        [JsonProperty("data")]
        public EInvoiceResponseDataModel Data { get; set; }

        public class EInvoiceResponseDataModel
        {
            [JsonProperty("url")]
            public string Url { get; set; }

           
            [JsonProperty("invoiceId")]
            public string InvoiceId { get; set; }

        
            [JsonProperty("invoiceKey")]
            public string InvoiceKey { get; set; }
        }
    }
}