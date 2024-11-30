namespace Disc.Fm.DataAccess.Contract.Services;

public interface ISettingsDataService
{
    Task<bool> UpdateCollection();
    bool ContainsKey(string key);
    string GetDiscogsUsername();
    Task<bool> UpdateDiscogsUsername(string userName);
    Task<bool> UpdateReleaseAddedOverTimeInitialExclusionPeriodInDays(string exclusionPeriod);
    Task<bool> PurgeEntireDb();
    Task<bool> UpdateLastFmSettings(string lastFmUsername, string lastFmPassword, string lastFmApiKey, string lastFmApiSecret);
    string GetLastFmApiKey();
    string GetLastFmApiSecret();
    string GetLastFmUsername();
    string GetLastFmPassword();
    string GetReleaseAddedOverTimeInitialExclusionPeriodInDays();
}

