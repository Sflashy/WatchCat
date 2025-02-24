using Newtonsoft.Json;
using System.Net.Http.Json;
using WatchCat.Core.Common;

namespace WatchCat.Core.Services;

public class HttpClientService
{
    private readonly HttpClient _httpClient;
    public HttpClientService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:135.0) Gecko/20100101 Firefox/135.0");
    }

    public async Task<IResult<T>> GetFromJsonAsync<T>(string url)
    {
        IResult<string> result = await GetStringAsync(url);
        if(!result.IsSuccess) return Result<T>.Failure(result.Message);
        return Result<T>.Success(JsonConvert.DeserializeObject<T>(result.Data));
    }

    public async Task<IResult<string>> GetStringAsync(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if(!response.IsSuccessStatusCode) return Result<string>.Failure("Failed to get data from the server.");
        return Result<string>.Success(await response.Content.ReadAsStringAsync());
    }
}
