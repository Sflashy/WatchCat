using System;
using System.Net.Http;
using System.Reflection;

namespace WatchCat
{
    public static class UpdateManager
    {
        public static async void CheckForUpdates()
        {
            using (var httpClient = new HttpClient())
            {
                string latestVersion = await httpClient.GetStringAsync("https://raw.githubusercontent.com/Sflashy/WatchCat/master/WatchCat/version.txt?token=GHSAT0AAAAAABYCL6KIXFQ7T5H3S7NIYMYSYYLYR5Q");
                string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                if(latestVersion != currentVersion)
                {
                    AppManager.ShowNotification($"A new version of WatchCat is available. Please update to version {latestVersion} now.", TimeSpan.FromSeconds(10));
                }
            }
        }
    }
}
