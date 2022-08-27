using Newtonsoft.Json;
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
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.", response.StatusCode.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                };
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
