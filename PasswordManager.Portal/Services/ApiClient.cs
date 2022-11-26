using System.Net.Http;

namespace PasswordManager.Portal.Services;

public sealed class ApiClient
{
    private readonly string _apiBaseAddress;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClientStateData _clientStateData;

    public ApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, ClientStateData clientStateData)
    {
        _httpClientFactory = httpClientFactory;
        _clientStateData = clientStateData;

        _apiBaseAddress = configuration.GetValue<string>("Api:BaseAddress")!;
    }

    public async Task<HttpResponseMessage> SendAnonymous(HttpRequestMessage requestMessage, string relativeUrl, CancellationToken cancellationToken)
    {
        requestMessage.RequestUri = GetUri(relativeUrl);
        requestMessage.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7210");

        var response = await GetClient().SendAsync(requestMessage, cancellationToken);

        return response;
    }

    public async Task<HttpResponseMessage> SendAuthorized(HttpRequestMessage requestMessage, string relativeUrl, CancellationToken cancellationToken)
    {
        requestMessage.RequestUri = GetUri(relativeUrl);
        requestMessage.Headers.Add("Authorization", $"Bearer {_clientStateData.User?.AccessToken}");
        requestMessage.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7210");

        var response = await GetClient().SendAsync(requestMessage, cancellationToken);

        return response;
    }

    public async Task<HttpResponseMessage> GetAuthorized(string relativeUrl, CancellationToken cancellationToken)
    {
        var client = GetClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_clientStateData.User?.AccessToken}");
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "https://localhost:7210");
        
        return await client.GetAsync(GetUri(relativeUrl), cancellationToken);
    }

    HttpClient GetClient()
    {
        var httpClient = _httpClientFactory.CreateClient();

        return httpClient;
    }

    private Uri GetUri(string relativeUrl)
    {
        if (relativeUrl.StartsWith('/'))
            relativeUrl = relativeUrl.Substring(1);

        string url = $"{_apiBaseAddress}{relativeUrl}";
        
        return new Uri(url);
    }
}
