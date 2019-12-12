using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestApp.ConsoleClientApp.WebClients
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(httpClient));
        }

        public async Task<IEnumerable<TEntity>> Get<TEntity, TEntityGetOptions>(TEntityGetOptions options, string url)
        {
            var response = await _httpClient
                .GetAsync(url)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(jsonData);

            return data;
        }

        public async Task Post<TEntity>(TEntity model, string url)
        {
            string json = JsonConvert.SerializeObject(model);

            var response = await _httpClient
                .PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Patch<TEntity>(TEntity model, string url)
        {
            string json = JsonConvert.SerializeObject(model);
            var response = await _httpClient
                .PatchAsync(url, new StringContent(json, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(string url)
        {
            var response = await _httpClient
                .DeleteAsync(url)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}