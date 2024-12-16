using Contact.Query.Domain.ResultServices;
using Contact.Query.Domain.Services;
using Serilog;
using System.Net.Http.Headers;

namespace Contact.Query.Infrastructure.ServicesAccess;
public class RegionQueryServiceApi(
    IHttpClientFactory httpClientFactory,
    ILogger logger) : Base, IRegionQueryServiceApi
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger _logger = logger;

    public async Task<Result<RegionResult>> RecoverByIdAsync(Guid id, string token)
    {
        _logger.Information($"{nameof(RecoverByIdAsync)} - Initiating call to Region.Query Api. Id: {id}.");

        var output = new Result<RegionResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("RegionQueryApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = string.Format("/api/v1/regionddd/recover-by-id/{0}", id);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseApi = DeserializeResponseObject<Result<RegionResult>>(content);

                _logger.Information($"{nameof(RecoverByIdAsync)} - Ended call to Region.Query Api. Id: {id}.");

                return responseApi;
            }

            var failMessage = $"{nameof(RecoverByIdAsync)} - An error occurred when calling the Region.Query Api. StatusCode: {response.StatusCode}";

            _logger.Error(failMessage);

            return output.Failure(failMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"{nameof(RecoverByIdAsync)} - An error occurred when calling the Region.Query Api. Error: {ex.Message}";

            _logger.Error(errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ListRegionResult>> RecoverListDDDByRegionAsync(string region, string token)
    {
        _logger.Information($"{nameof(RecoverByIdAsync)} - Initiating call to Region.Query Api.");

        var output = new Result<ListRegionResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("RegionQueryApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = string.Format("/api/v1/regionddd/ddd/by-region?region={0}", region);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseApi = DeserializeResponseObject<Result<ListRegionResult>>(content);

                _logger.Information($"{nameof(RecoverByIdAsync)} - Ended call to Region.Query Api.");

                return responseApi;
            }

            var failMessage = $"{nameof(RecoverByIdAsync)} - An error occurred when calling the Region.Query Api. StatusCode: {response.StatusCode}";

            _logger.Error(failMessage);

            return output.Failure(failMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"{nameof(RecoverByIdAsync)} - An error occurred when calling the Region.Query Api. Error: {ex.Message}";

            _logger.Error(errorMessage);

            return output.Failure(errorMessage);
        }
    }
    public async Task<Result<RegionResult>> RecoverByDDDAsync(int ddd, string token)
    {
        _logger.Information($"{nameof(RecoverByDDDAsync)} - Initiating call to Region.Query Api.");

        var output = new Result<RegionResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("RegionQueryApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uri = string.Format("/api/v1/regionddd/recover-by-ddd/{0}", ddd);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var responseApi = DeserializeResponseObject<Result<RegionResult>>(content);

                _logger.Information($"{nameof(RecoverByDDDAsync)} - Ended call to Region.Query Api.");

                return responseApi;
            }

            var failMessage = $"{nameof(RecoverByDDDAsync)} - An error occurred when calling the Region.Query Api. StatusCode: {response.StatusCode}";

            _logger.Error(failMessage);

            return output.Failure(failMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = $"{nameof(RecoverByDDDAsync)} - An error occurred when calling the Region.Query Api. Error: {ex.Message}";

            _logger.Error(errorMessage);

            return output.Failure(errorMessage);
        }
    }
}
