using System;
using System.Threading.Tasks;
using AirPortLogic.Infra;
using Models.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AirPortLogic.Service
{
    class TowerHttpClientService : ITowerHttpClientService
    {
        private int tryCount =0;

        HttpClient _client;

        HttpClient Client { get { return _client ?? (InitClient()); } }

        private HttpClient InitClient()
        {
            try
            {
                _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:59154/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
            }
            catch (Exception)
            {
                tryCount = 5;
                return null;
            }
        }

        public async Task<bool> ReturnPlaneToFactory(Plane plane)
        {
            if (tryCount < 4)
                try
                {
                    var content = JsonConvert.SerializeObject(plane);
                    var response = await Client.PostAsync("api/factory", new StringContent(content));
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    return false;
                }
            else
                return false;
        }
    }
}
