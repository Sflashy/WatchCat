using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WatchCat
{
    public static class AppManager
    {
        public static async Task<dynamic> HttpRequest(string url)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                while (!response.IsSuccessStatusCode)
                {
                    Items.Instance.Snackbar.MessageQueue.Enqueue("The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.",
                    null, null, null, false, true, TimeSpan.FromMinutes(1));
                    await Task.Delay(new TimeSpan(0, 1, 0));
                    response = await httpClient.GetAsync(url);
                    
                }
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
