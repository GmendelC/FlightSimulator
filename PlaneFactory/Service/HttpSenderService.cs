using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PlaneFactory.Service
{
    public class HttpSenderService
    {
        // send http request to airport
        HttpClient _client;
        private bool _invalid;

        HttpClient Client { get { return _client ?? (InitClient()); } }

        private HttpClient InitClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:6462");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
        }

        internal async Task<bool> SendFlight(Fligth newFlight)
        {
            if (!_invalid)
                try
                {
                    var response = await Client.PostAsJsonAsync("api/Flight", newFlight);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    _invalid = true;
                    Debug.WriteLine(ex);
                    return false;
                }
            else
                return false;
        }
    }
}