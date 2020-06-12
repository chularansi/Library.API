using Blazored.LocalStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Library.UI.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ILocalStorageService localStorage;

        public BaseRepository(IHttpClientFactory httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<bool> Create(string url, T obj)
        {
            if(obj == null)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var client = httpClient.CreateClient();

            // Make sure the user is authorized to carry out the action
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<IList<T>> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }

            return null;
        }

        public async Task<T> Get(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            if (obj == null)
            {
                return false;
            }
            
            var request = new HttpRequestMessage(HttpMethod.Put, url + id);
            request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            var client = httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetBearerToken());

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        private async Task<string> GetBearerToken()
        {
            return await localStorage.GetItemAsync<string>("authToken");
        }
    }
}
