using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchCat
{
    public static class AppManager
    {
        public static async Task<dynamic> HttpRequest(string url)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
