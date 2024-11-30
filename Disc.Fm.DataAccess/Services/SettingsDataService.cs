using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class SettingsDataService : ISettingsDataService
{
    private readonly ISQLiteAsyncConnection _db;
    private readonly ICollectionDataService _collectionDataService;
    private readonly IPreferencesService _preferencesService;

    public SettingsDataService(ISQLiteAsyncConnection db,  IPreferencesService preferencesService, ICollectionDataService collectionDataService)
    {
        _db = db;
        _preferencesService = preferencesService;
        _collectionDataService = collectionDataService;
    }

    public async Task<bool> UpdateCollection()
    {
        return await _collectionDataService.UpdateCollectionFromDiscogs();
    }

    public string GetDiscogsUsername()
    {
        var username = _preferencesService.Get(PreferencesConstant.DiscogsUsername, "");
        return username;
    }

    public bool ContainsKey(string key)
    {
        return _preferencesService.ContainsKey(key);
    }

    public async Task<bool> UpdateDiscogsUsername(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            _preferencesService.Remove(PreferencesConstant.DiscogsUsername);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.DiscogsUsername, userName);
        }
        await PurgeEntireDb();
        return true;
    }
    public async Task<bool> UpdateReleaseAddedOverTimeInitialExclusionPeriodInDays(string exclusionPeriod)
    {
        if (string.IsNullOrEmpty(exclusionPeriod))
        {
            _preferencesService.Remove(PreferencesConstant.ReleaseAddedOverTimeInitialExclusionPeriodInDays);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.ReleaseAddedOverTimeInitialExclusionPeriodInDays, exclusionPeriod);
        }

        return true;
    }

    public async Task<bool> PurgeEntireDb()
    {
        await _db.PurgeEntireDb();
        return true;
    }

    public Task<bool> UpdateLastFmSettings(string lastFmUsername, string lastFmPassword, string lastFmApiKey, string lastFmApiSecret)
    {
        if (string.IsNullOrEmpty(lastFmUsername))
        {
            _preferencesService.Remove(PreferencesConstant.LastFmUserName);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.LastFmUserName, lastFmUsername);
        }

        if (string.IsNullOrEmpty(lastFmPassword))
        {
            _preferencesService.Remove(PreferencesConstant.LastFmPassword);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.LastFmPassword, lastFmPassword);
        }

        if (string.IsNullOrEmpty(lastFmApiKey))
        {
            _preferencesService.Remove(PreferencesConstant.LastFmApiKey);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.LastFmApiKey, lastFmApiKey);
        }
        
        if (string.IsNullOrEmpty(lastFmApiSecret))
        {
            _preferencesService.Remove(PreferencesConstant.LastFmApiSecret);
        }
        else
        {
            _preferencesService.Set(PreferencesConstant.LastFmApiSecret, lastFmApiSecret);
        }

        return Task.FromResult(true);
    }

    public string GetLastFmApiKey()
    {
        var key = _preferencesService.Get(PreferencesConstant.LastFmApiKey, "");
        return key;
    }

    public string GetLastFmApiSecret()
    {
        var key = _preferencesService.Get(PreferencesConstant.LastFmApiSecret, "");
        return key;
    }

    public string GetLastFmUsername()
    {
        var username = _preferencesService.Get(PreferencesConstant.LastFmUserName, "");
        return username;
    }

    public string GetLastFmPassword()
    {
        var pass = _preferencesService.Get(PreferencesConstant.LastFmPassword, "");
        return pass;
    }

    public string GetReleaseAddedOverTimeInitialExclusionPeriodInDays()
    {
        var releaseAddedOverTimeInitialExclusionPeriodInDays = _preferencesService.Get(PreferencesConstant.ReleaseAddedOverTimeInitialExclusionPeriodInDays, "");
        return releaseAddedOverTimeInitialExclusionPeriodInDays;
    }
}
