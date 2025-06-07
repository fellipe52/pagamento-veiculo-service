using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Pagamento.Domain.Contracts;
using Pagamento.Domain.Interfaces.Service;

namespace Pagamento.Infrastructure.Service
{
    public class RedeItauPagamentoClient : IRedeItauPagamento
    {
        private readonly string _pv;
        private readonly string _token;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://sandbox-erede.useredecloud.com.br/v1";

        public RedeItauPagamentoClient(string pv, string token)
        {
            _pv = pv;
            _token = token;
            _httpClient = new HttpClient();

            var authBytes = Encoding.ASCII.GetBytes($"{_pv}:{_token}");
            var authBase64 = Convert.ToBase64String(authBytes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase64);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TransactionsResponse> CriarTransacaoAsync(TransactionsRquest request)
        {
            TransactionsResponse? response = null;
            var url = $"{_baseUrl}/transactions";
            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(url, content);
            var responseBody = await result.Content.ReadAsStringAsync();


            response = JsonConvert.DeserializeObject<TransactionsResponse>(responseBody);


            if (!result.IsSuccessStatusCode)
            {
                return response;
            }

            return response;
        }
    }
}