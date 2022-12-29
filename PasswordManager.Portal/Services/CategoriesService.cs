using LanguageExt.Common;
using PasswordManager.Portal.DtObjects;
using PasswordManager.Portal.ViewModels.AddPassword;
using PasswordManager.Portal.ViewModels.Dashboard;
using System.Text.Json;

namespace PasswordManager.Portal.Services;

public sealed class CategoriesService
{
    private readonly ApiClient _apiClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CategoriesService(ApiClient apiClient, JsonSerializerOptions jsonSerializerOptions)
	{
        _apiClient = apiClient;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<Result<List<AvailableCategory>>> GetAllCategories(CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };

            var response = await _apiClient.GetAuthorized("/api/Categories", cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStreamAsync();

            var responseModel = await JsonSerializer.DeserializeAsync<List<CategoryResponseModel>>(responseContent, _jsonSerializerOptions);

            if (responseModel is null)
            {
                return new Result<List<AvailableCategory>>(new Exception($"Wrong model {nameof(AvailableCategory)} is null"));
            }

            var categoryModels = responseModel.Select(r => new AvailableCategory
            {
                Id= r.Id,
                Name = r.Title,
                Description = r.Description
            }).ToList();

            return categoryModels;
        }
        catch (Exception ex)
        {
            return new Result<List<AvailableCategory>>(ex);
        }
    }
}
