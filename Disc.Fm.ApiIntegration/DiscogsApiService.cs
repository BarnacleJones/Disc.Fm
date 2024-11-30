using Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels;
using Disc.Fm.ApiIntegration.Contract.Services;
using Disc.Fm.DataAccess.Contract.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Disc.Fm.ApiIntegration
{
    public class DiscogsApiService : IDiscogsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DiscogsApiService> _logger;
        private readonly IPreferencesService _preferencesService;
        private string _discogsUserName;

        //-----------------------------------------------------------------------------
        //Hard coded Disgogs API Endpoints. If they ever change this should be the only place they are referenced!

        //If they take a parameter (Eg ArtistId, use value from entity prefixed with Discogs eg. DiscogsArtistId

        private const string ReleaseUrl = "/releases/";
        private const string MasterReleaseUrl = "/masters/";
        private const string ArtistUrl = "/artists/";

        //collection url is in GetCollectionFromDiscogsApi function

        //-----------------------------------------------------------------------------


        public DiscogsApiService(IHttpClientFactory httpClientFactory, ILogger<DiscogsApiService> logger, IPreferencesService preferencesService)
        {
            _httpClient = httpClientFactory.CreateClient("DiscogsApiClient");
            _logger = logger;
            _preferencesService = preferencesService;
        }

        public async Task<DiscogsCollectionResponse> GetCollectionFromDiscogsApi()
        {
            try
            {
                //API can handle 25 requests per minute
                var releases = new List<ResponseRelease>();
                var currentPage = 1;
                var totalPages = 1;
                var responseData = new DiscogsCollectionResponse();
                _discogsUserName = _preferencesService.Get(PreferencesConstant.DiscogsUsername, "Unknown");
                if (string.IsNullOrEmpty(_discogsUserName))
                {
                    _logger.LogError("Empty username");
                    return new DiscogsCollectionResponse();
                }
                var collectionUrl = $"/users/{_discogsUserName}/collection/releases/0?page={currentPage}&per_page=1000";//500 is max but hey

                do
                {
                    var response = await _httpClient.GetAsync(collectionUrl);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    responseData = JsonSerializer.Deserialize<DiscogsCollectionResponse>(json);

                    if (responseData == null)
                        throw new Exception("Error getting data");
                    if (responseData.releases == null) //no releases in collection
                        return new DiscogsCollectionResponse();

                    releases.AddRange(responseData.releases);

                    totalPages = responseData?.pagination?.pages ?? 1;
                    currentPage++;

                } while (currentPage <= totalPages);

                if (releases is null)
                    throw new Exception("Error getting data");

                return new DiscogsCollectionResponse { releases = releases };
            }
            catch
            {
                throw;
            }
        }

        public async Task<DiscogsReleaseResponse> GetReleaseFromDiscogs(int discogsReleaseId)
        {
            try
            {
                var thisReleaseUrl = ReleaseUrl + discogsReleaseId;
                var response = await _httpClient.GetAsync(thisReleaseUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<DiscogsReleaseResponse>(json);

                return responseData == null
                    ? throw new Exception("Error getting data")
                    : responseData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get data from discogs API: {ex.Message}");
            }
        }

        public async Task<DiscogsArtistResponse> GetArtistFromDiscogs(int discogsArtistId)
        {
            try
            {
                var thisArtistUrl = ArtistUrl + discogsArtistId;
                var response = await _httpClient.GetAsync(thisArtistUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<DiscogsArtistResponse>(json);

                return responseData == null
                    ? throw new Exception("Error getting data")
                    : responseData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get data from discogs API: {ex.Message}");
            }
        }
    }
}
