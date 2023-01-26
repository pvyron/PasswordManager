using LanguageExt.Pipes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using PasswordManager.Portal.Constants;

namespace PasswordManager.Portal.Services;

public sealed class ApiClient
{
    private readonly string _apiBaseAddress;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClientStateData _clientStateData;
    private readonly NavigationManager _navigationManager;

    public ApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, ClientStateData clientStateData, NavigationManager navigationManager)
    {
        _httpClientFactory = httpClientFactory;
        _clientStateData = clientStateData;
        _navigationManager = navigationManager;
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
        return await SendAuthorized(requestMessage, relativeUrl, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    public async Task<HttpResponseMessage> SendAuthorized(HttpRequestMessage requestMessage, string relativeUrl, HttpCompletionOption httpCompletionOption, CancellationToken cancellationToken)
    {
        requestMessage.RequestUri = GetUri(relativeUrl);
        requestMessage.Headers.Add("Authorization", $"Bearer {_clientStateData.User?.AccessToken}");
        requestMessage.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7210");
        requestMessage.SetBrowserResponseStreamingEnabled(true);

        var response = await GetClient().SendAsync(requestMessage, httpCompletionOption, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _clientStateData.Logout();
            _navigationManager.NavigateTo(ApplicationRoutes.Login);
        }

        return response;
    }

    public async Task<HttpResponseMessage> GetAuthorized(string relativeUrl, CancellationToken cancellationToken)
    {
        return await GetAuthorized(relativeUrl, HttpCompletionOption.ResponseContentRead, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetAuthorized(string relativeUrl, HttpCompletionOption httpCompletionOption, CancellationToken cancellationToken)
    {
        var client = GetClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_clientStateData.User?.AccessToken}");
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "https://localhost:7210");

        var response = await client.GetAsync(GetUri(relativeUrl), httpCompletionOption, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _clientStateData.Logout();
            _navigationManager.NavigateTo(ApplicationRoutes.Login);
        }

        return response;
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
