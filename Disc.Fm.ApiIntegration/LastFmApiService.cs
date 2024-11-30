using Disc.Fm.ApiIntegration.Contract;
using Disc.Fm.ApiIntegration.Contract.Services;
using Disc.Fm.DataAccess.Contract.Services;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using Newtonsoft.Json;

namespace Disc.Fm.ApiIntegration;

public class LastFmApiService : ILastFmApiService
{
    private readonly LastfmClient _lastFmClient;
    private readonly IPreferencesService _preferencesService;
    private readonly HttpClient _httpClient;
    private string _lastFmUserName;
    private string _lastFmPassword;
    private string _lastFmApiKey;
    private string _lastFmApiSecret;

    public LastFmApiService(IPreferencesService preferencesService, IHttpClientFactory httpClientFactory)
    {
        _preferencesService = preferencesService;
        _httpClient = httpClientFactory.CreateClient("LastFmApiClient");
        _lastFmApiKey = _preferencesService.Get(PreferencesConstant.LastFmApiKey, "");
        _lastFmApiSecret = _preferencesService.Get(PreferencesConstant.LastFmApiSecret, "");
        _lastFmUserName = _preferencesService.Get(PreferencesConstant.LastFmUserName, "");
        _lastFmPassword = _preferencesService.Get(PreferencesConstant.LastFmPassword, "");

        _lastFmClient = new LastfmClient(_lastFmApiKey, _lastFmApiSecret, _httpClient);
    }

    public async Task<LastAlbumResponseManual> GetAlbumInformation(string artistName, string albumName)
    {
        await EnsureAuthenticatedAsync();
        var encodedArtistName = Uri.EscapeDataString(artistName);
        var encodedAlbumName = Uri.EscapeDataString(albumName);
        //have to use regular getAsync method (insead of using LastFmClient) for Android to work - needs to be HTTPS.
        //I dont see a way IF.Lastfm.Core supports that at this stage
        var url = $"https://ws.audioscrobbler.com/2.0/?method=album.getInfo&api_key={_lastFmApiKey}&artist={encodedArtistName}&album={encodedAlbumName}&format=json";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        try
        {
            var albumInfo = JsonConvert.DeserializeObject<LastAlbumResponseManual>(json);
            return albumInfo;
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            throw new Exception($"Error deserializing JSON: {ex.Message}");
        }

    }

    public async Task<LastResponse> ScrobbleRelease(Scrobble scrobble)
    {
        await EnsureAuthenticatedAsync();
        var scrobbleResponse = await _lastFmClient.Scrobbler.ScrobbleAsync(scrobble);

        return scrobbleResponse ?? throw new Exception("Error scrobbling release");
    }

    public async Task<LastResponse> ScrobbleReleases(List<Scrobble> scrobbles)
    {
        await EnsureAuthenticatedAsync();
        var scrobbleResponse = await _lastFmClient.Scrobbler.ScrobbleAsync(scrobbles);

        return scrobbleResponse ?? throw new Exception("Error scrobbling release");
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (!_lastFmClient.Auth.Authenticated)
        {
            var authenticationResponse = await _lastFmClient.Auth.GetSessionTokenAsync(_lastFmUserName, _lastFmPassword);

            if (!authenticationResponse.Success)
            {
                throw new Exception($"Authentication failed: {authenticationResponse.Status}");
            }
        }
    }
}
