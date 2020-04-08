using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Services.Parsers;

namespace Application.Services.Web
{
    public class ClientRestAdapter : APIRest
    {
        private readonly HttpClient _client;
        private readonly JsonParser _jsonParser;

        public ClientRestAdapter(JsonParser jsonParser)
        {
            _jsonParser = jsonParser;
            _client = new HttpClient();
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response
        {
            const string jsonMediaType = "application/json";
            var data = new StringContent(_jsonParser.ToJson(request), Encoding.UTF8, jsonMediaType);
            var response = await _client.PostAsync(url, data);
            var contents = await response.Content.ReadAsStringAsync();

            return _jsonParser.FromJson<TResponse>(contents);
        }

        public async Task<TResponse> Get<TRequest, TResponse>(string url, TRequest request)
            where TRequest : Request where TResponse : Response
        {
            var uri = new Uri(url);
            var finalUri = uri.ExtendQuery(request);

            var response = await _client.GetAsync((Uri) finalUri);
            var contents = await response.Content.ReadAsStringAsync();

            return _jsonParser.FromJson<TResponse>(contents);
        }
    }
}