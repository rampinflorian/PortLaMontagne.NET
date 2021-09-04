using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PortLaMontagne.Services.API
{
    public class OpenWeatherMapApi
    {
        public async Task<string> CallApi()
        {
            var uriBuilder = new UriBuilder("https://api.openweathermap.org/data/2.5/weather/");
            uriBuilder.Query = "q=TOULON&appid=323462b97bb0ac337f334e86128cfe01";

            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(uriBuilder.Uri).Result;
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }
    }
}