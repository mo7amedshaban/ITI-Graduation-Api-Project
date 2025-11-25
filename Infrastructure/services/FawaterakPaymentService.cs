using Application.Common.Interfaces;
using Application.DTOs.Payment;
using Core.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Application.Common.Models.EInvoiceResponseModel;
using static Core.DTOs.PaymentMethodsResponse;

namespace Infrastructure.services
{
    public class FawaterakPaymentService : IFawaterakPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string ApiKey;
        private readonly string BaseUrl;
        private readonly string ProviderKey;


        public FawaterakPaymentService(IHttpClientFactory httpClientFactory, IOptions<FawaterakOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            var cfg = options.Value;
            ApiKey = cfg.ApiKey;
            BaseUrl = cfg.BaseUrl;
            ProviderKey = cfg.ProviderKey;

        }

        #region Create EInvoice Link

        public async Task<EInvoiceResponseDataModel?> CreateEInvoiceAsync(EInvoiceRequestModel eInvoice)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/createInvoiceLink");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new StringContent(JsonConvert.SerializeObject(eInvoice), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var _response = JsonConvert.DeserializeObject<Application.Common.Models.EInvoiceResponseModel>(responseContent);
                return _response?.Data;
            }
            else
            {

                var errorContent = await response.Content.ReadAsStringAsync();
                // _logger.LogError("Fawaterak API error: Status {StatusCode}, Response: {ErrorContent}",
                //response.StatusCode, errorContent);

                throw new Exception($"Fawaterak API error: {response.StatusCode}. {errorContent}");
            }


        }
        #endregion

        #region Payment Integration

        #region Payment Methods
        public async Task<IList<PaymentMethodsResponse.PaymentMethod>?> GetPaymentMethods()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/getPaymentmethods");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var result = await client.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                var _response = JsonConvert.DeserializeObject<PaymentMethodsResponse>(responseContent);

                foreach (var item in _response.Data)
                {
                    item.Id = (int)(await GetPaymentMethod(item.PaymentId, _response.Data));
                }
                return _response!.Data!;
            }

            return null;
        }

        public async Task<Core.DTOs.PaymentMethod> GetPaymentMethod(int paymentMethodId, IList<PaymentMethodsResponse.PaymentMethod>? paymentMethods = null)
        {
            var methods = paymentMethods ?? await GetPaymentMethods();

            var method = methods?.FirstOrDefault(x => x.PaymentId == paymentMethodId);
            if (string.IsNullOrWhiteSpace(method?.NameEn))
                return Core.DTOs.PaymentMethod.CreditCard;

            var name = method.NameEn;

            //if (name.Contains("Fawry", StringComparison.OrdinalIgnoreCase))
            //    return PaymentMethods.Fawry;

            if (name.Contains("Meeza", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("Wallet", StringComparison.OrdinalIgnoreCase))
                return Core.DTOs.PaymentMethod.Wallet;

            return Core.DTOs.PaymentMethod.CashOnDelivery;
        }

        #endregion

        public async Task<BasePaymentDataResponse?> GeneralPay(EInvoiceRequestModel invoice)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/invoiceInitPay");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");
            invoice.Customer.Email = NormalizeEmailDots(invoice.Customer.Email);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var method = await GetPaymentMethod(invoice.PaymentMethodId.Value);

                switch (method)
                {
                    //case Domain.Enums.PaymentMethod.:
                    //    var _fawryResponse = JsonConvert.DeserializeObject<FawryPaymentResponse>(responseContent);
                    //    return _fawryResponse!.Data;
                    case Core.DTOs.PaymentMethod.Wallet:
                        var _meezaResponse = JsonConvert.DeserializeObject<MeezaPaymentResponse>(responseContent);
                        return _meezaResponse!.Data;
                    case Core.DTOs.PaymentMethod.CreditCard:
                        var _cardResponse = JsonConvert.DeserializeObject<CardPaymentResponse>(responseContent);
                        return _cardResponse!.Data;
                    default:
                        return null;
                }
            }

            var errorResponseContent = await response.Content.ReadAsStringAsync();
            return null;
        }
        #endregion

        #region WebHook Verification
        public bool VerifyWebhook(WebHookModel webHook)
        {
            var generatedHashKey =
                GenerateHashKeyForWebhookVerification(webHook.InvoiceId, webHook.InvoiceKey, webHook.PaymentMethod);
            return generatedHashKey == webHook.HashKey;
        }

        public bool VerifyCancelTransaction(CancelTransactionModel cancelTransaction)
        {
            var generatedHashKey = GenerateHashKeyForCancelTransaction(cancelTransaction.ReferenceId, cancelTransaction.PaymentMethod);
            return generatedHashKey == cancelTransaction.HashKey;
        }

        public bool VerifyApiKeyTransaction(string apiKey)
        {
            return apiKey == ApiKey;
        }
        #endregion

        #region Generate HashKey
        public string GenerateHashKeyForIFrame(string domain)
        {
            var queryParam = $"Domain={domain}&ProviderKey={ProviderKey}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private string GenerateHashKeyForWebhookVerification(long invoiceId, string invoiceKey, string paymentMethod)
        {
            var queryParam = $"InvoiceId={invoiceId}&InvoiceKey={invoiceKey}&PaymentMethod={paymentMethod}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private string GenerateHashKeyForCancelTransaction(string referenceId, string paymentMethod)
        {
            var queryParam = $"referenceId={referenceId}&PaymentMethod={paymentMethod}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static string NormalizeEmailDots(string email)
        {
            if (string.IsNullOrEmpty(email)) return email;

            int atIndex = email.IndexOf('@');
            if (atIndex < 0) return email;

            string localPart = email.Substring(0, atIndex);
            string domainPart = email.Substring(atIndex);

            localPart = System.Text.RegularExpressions.Regex.Replace(localPart, @"\.+", ".");

            return localPart + domainPart;
        }
        #endregion
    }
}
