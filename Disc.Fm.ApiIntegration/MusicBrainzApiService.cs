using Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;
using Disc.Fm.ApiIntegration.Contract.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Disc.Fm.ApiIntegration;

public class MusicBrainzApiService : IMusicBrainzApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MusicBrainzApiService> _logger;

    //-----------------------------------------------------------------------------
    //Hard coded MusicBrainz API Endpoints. If they ever change this should be the only place they are referenced!

    //MusicBrainzApi documentation: https://musicbrainz.org/doc/MusicBrainz_API


    private const string InitialArtistRequest = "/ws/2/artist/?query=artist:";

    //know about release groups - thats what you want at this stage, its the main release info https://musicbrainz.org/doc/Release_Group

    private const string ReleaseGroupUrl = "/ws/2/release-group/";
    private const string ReleaseGroupIncludeUrl = "?inc=artists+releases";

    private const string ReleaseUrl = "/ws/2/release/";
    private const string ReleaseIncludeUrl = "?inc=artist-credits+labels+discids+recordings+tags";

    private const string ArtistUrl = "/ws/2/artist/";
    private const string ArtistIncludeUrl = "?inc=aliases+releases+release-groups";

    //-----------------------------------------------------------------------------


    public MusicBrainzApiService(IHttpClientFactory httpClientFactory, ILogger<MusicBrainzApiService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("MusicBrainzApiClient");
        _logger = logger;
    }

    public async Task<MusicBrainzInitialArtist> GetInitialArtistFromMusicBrainzApi(string artistName)
    {
        try
        {
            var responseData = new MusicBrainzInitialArtist();

            //get really bad data when artist has discogs (number) (eg conflict(2))
            artistName = RemoveNumericParentheses(artistName);

            var encodedArtistName = Uri.EscapeDataString(artistName);

            var fullArtistRequestUrl = $"{InitialArtistRequest}\"{encodedArtistName}\"";

            var response = await _httpClient.GetAsync(fullArtistRequestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            responseData = JsonSerializer.Deserialize<MusicBrainzInitialArtist>(json);

            return responseData == null
                ? throw new Exception("Error getting musicbrainz artist data")
                : responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data from API");
            throw;
        }
    }

    private string RemoveNumericParentheses(string input)
    {
        //removes parenthesis info when there are numbers within parenthesis
        //are there bands with BandName(1) types of names? time will tell...
        return Regex.Replace(input, @"\(\d+\)", "").Trim();
    }
    public async Task<MusicBrainzArtist> GetArtistFromMusicBrainzApiUsingArtistId(string musicBrainzArtistId)
    {
        try
        {
            var responseData = new MusicBrainzArtist();

            var fullArtistRequestUrl = ArtistUrl + $"{musicBrainzArtistId}" + ArtistIncludeUrl;

            var response = await _httpClient.GetAsync(fullArtistRequestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            responseData = JsonSerializer.Deserialize<MusicBrainzArtist>(json);

            return responseData == null
                ? throw new Exception("Error getting musicbrainz artist data")
                : responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data from API");
            throw;
        }
    }
    public async Task<MusicBrainzRelease> GetReleaseFromMusicBrainzApiUsingMusicBrainsReleaseId(string musicBrainzReleaseId)
    {
        try
        {
            var responseData = new MusicBrainzRelease();

            var fullReleaseRequestUrl = ReleaseUrl + $"{musicBrainzReleaseId}" + ReleaseIncludeUrl;

            var response = await _httpClient.GetAsync(fullReleaseRequestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            responseData = JsonSerializer.Deserialize<MusicBrainzRelease>(json);

            return responseData == null
                ? throw new Exception("Error getting musicbrainz release data")
                : responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data from API");
            throw;
        }
    }

    public async Task<MusicBrainzReleaseGroup> GetReleaseGroupFromMusicBrainzApiUsingMusicBrainsReleaseId(string musicBrainzReleaseId)
    {
        try
        {
            var responseData = new MusicBrainzReleaseGroup();

            var fullReleaseRequestUrl = ReleaseGroupUrl + $"{musicBrainzReleaseId}" + ReleaseGroupIncludeUrl;

            var response = await _httpClient.GetAsync(fullReleaseRequestUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            responseData = JsonSerializer.Deserialize<MusicBrainzReleaseGroup>(json);

            return responseData == null
                ? throw new Exception("Error getting musicbrainz release group data")
                : responseData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data from API");
            throw;
        }
    }
}
