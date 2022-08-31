using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;

namespace WatchCat
{
    public static class AppManager
    {
        public delegate void ConnectionFailed();
        public static event ConnectionFailed OnConnectionFailed;
        public static async Task<dynamic> HttpRequest(string url)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    ShowNotification("The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.", TimeSpan.FromSeconds(10));
                    OnConnectionFailed?.Invoke();
                    return null;
                }
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            }
        }

        public static void ShowNotification(string message, TimeSpan duration)
        {
            MainWindow.Instance.Snackbar.MessageQueue.Enqueue(message,
            null, null, null, false, true, duration);
        }
    }
}
