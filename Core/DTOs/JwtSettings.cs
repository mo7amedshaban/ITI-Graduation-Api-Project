using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
}

public class AuthResult
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string SessionId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }
}

public enum PaymentMethod
{
    CashOnDelivery = 1,
    CreditCard = 2,
    Wallet = 3,
    PayPal = 4

}
public class EInvoiceRequestModel
{

    [JsonProperty("payment_method_id")]
    public int? PaymentMethodId { get; set; }

    [JsonProperty("customer")]
    [Required]
    public required CustomerModel Customer { get; set; }


    [JsonProperty("cartItems")]
    [MinLength(1)]
    [Required]
    public List<CartItemModel> CartItems { get; set; }


    [JsonProperty("cartTotal")]
    public decimal CartTotal => CartItems.Sum(item => item.Price * item.Quantity);


    [JsonProperty("currency")]
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "EGP";


    [JsonProperty("payLoad")]
    public InvoicePayload? PayLoad { get; set; }

    //[JsonProperty("invoice_number")]
    //public string? InvoiceNumber { get; set; } // nrw


    //[JsonProperty("redirectionUrls")]
    //public RedirectionUrlsModel? RedirectionUrls { get; set; }


    public class InvoicePayload
    {
        public string OrderId { get; set; }
    }


    public class CustomerModel
    {

        [JsonProperty("customer_unique_id")]
        public string? CustomerId { get; set; }


        [JsonProperty("first_name")]
        [Required]
        public string? FirstName { get; set; }


        [JsonProperty("last_name")]
        [Required]
        public string? LastName { get; set; }


        [JsonProperty("email")]
        [EmailAddress]
        public string? Email { get; set; }


        [JsonProperty("phone")]
        [Phone]
        public string? Phone { get; set; }
    }


    public class CartItemModel
    {

        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("price")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }


    public class RedirectionUrlsModel
    {

        [JsonProperty("successUrl")]
        [Url]
        public string? OnSuccess { get; set; }

        [JsonProperty("failUrl")]
        [Url]
        public string? OnFailure { get; set; }

        [JsonProperty("pendingUrl")]
        [Url]
        public string? OnPending { get; set; }
    }
}

public class PaymentMethodsResponse
{

    [JsonProperty("status")]
    public string Status { get; set; }


    [JsonProperty("data")]
    public List<PaymentMethod> Data { get; set; }


    public class PaymentMethod
    {

        public int Id { get; set; }


        [JsonProperty("paymentId")]
        public int PaymentId { get; set; }


        [JsonProperty("name_en")]
        public string NameEn { get; set; }


        [JsonProperty("name_ar")]
        public string NameAr { get; set; }


        [JsonProperty("redirect")]
        public string Redirect { get; set; }


        [JsonProperty("logo")]
        public string Logo { get; set; }

    }


    public sealed class FawaterakOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;


        //public RedirectionUrlsModel RedirectionUrls { get; set; }

        //public string successUrl { get; set; }
        //public string failUrl { get; set; }
        //public string pendingUrl { get; set; }
    }
}



