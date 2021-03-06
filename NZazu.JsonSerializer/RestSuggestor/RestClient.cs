using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NZazu.JsonSerializer.RestSuggestor
{
    public class RestClient : IRestClient, IDisposable
    {
        internal static readonly JObject EmptyJson = new JObject();
        private readonly IHttpClient _httpClient;
        private readonly List<IHookHttpRequest> _requestHooks;

        public RestClient(IHttpClient httpClient = null, IEnumerable<IHookHttpRequest> requestHooks = null)
        {
            _httpClient = httpClient ?? new HttpClientWrapper();
            _requestHooks = (requestHooks ?? Enumerable.Empty<IHookHttpRequest>()).ToList();
        }

        public bool ThrowOnErrors { get; set; } = true;

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<JToken> Request(HttpMethod method = null, string uri = null, JToken body = null)
        {
            var httpMethod = method ?? HttpMethod.Get;
            var request = new HttpRequestMessage(httpMethod, uri);
            _requestHooks.ForEach(h => h.Hook(request));
            if (body != null)
                request.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.Content == null) return EmptyJson;
            var content = await response.Content.ReadAsStringAsync();
            var json = !string.IsNullOrWhiteSpace(content) ? JToken.Parse(content) : EmptyJson;
            if (ThrowOnErrors) json.ThrowOnErrors();
            return json;
        }

        public Uri BaseAddress
        {
            get => _httpClient.BaseAddress;
            set => _httpClient.BaseAddress = value;
        }
    }
}