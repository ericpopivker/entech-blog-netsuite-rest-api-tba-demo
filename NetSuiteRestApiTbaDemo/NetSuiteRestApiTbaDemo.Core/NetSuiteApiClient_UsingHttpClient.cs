using System.Text;
using System.Text.Json;

using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Diagnostics;
using RestSharp;

namespace NetSuiteRestApiTbaDemo.Core
{

    public class NetSuiteApiClient_UsingHttpClient
    {

        private static NetSuiteApiConfig _config;
        private static readonly HttpClient _httpClient;

        static NetSuiteApiClient_UsingHttpClient()
        {
            _config = new NetSuiteApiConfig();
            _httpClient = new HttpClient();
        }

        public async Task<List<string>> FindCustomerIds(int limit)
        {
            var url = _config.ApiRoot + "/customer?limit=" + limit;

            using var httpRequest = CreateHttpRequestMessage(HttpMethod.Get, url);
            

            var httpResponse = await _httpClient.SendAsync(httpRequest);
            var responseJson = await httpResponse.Content.ReadAsStringAsync();

            var response =
                JsonSerializer.Deserialize<NsFindIdsResponse>(responseJson);

            return response.items.Select(i => i.id).ToList();
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string requestUrl)
        {
            var oauth1 = new OAuth1HeaderGenerator(_config, httpMethod, requestUrl);

            var httpRequest = new HttpRequestMessage(httpMethod, requestUrl);
            httpRequest.Headers.Authorization = oauth1.CreateAuthenticationHeaderValue();

            return httpRequest;
        }

        public async Task<NsCustomer> GetCustomer(int customerId)
        {
            var url = _config.ApiRoot + "/customer/" + customerId;

            using var httpRequest = CreateHttpRequestMessage(HttpMethod.Get, url);

            var httpResponse = await _httpClient.SendAsync(httpRequest);
            var responseJson = await httpResponse.Content.ReadAsStringAsync();

            var customer =
                 JsonSerializer.Deserialize<NsCustomer>(responseJson);

            return customer;
        }
    }
}