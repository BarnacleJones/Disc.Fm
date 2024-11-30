using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class PreferencesService : IPreferencesService
{
    public string Get(string key, string defaultValue)
    {
        return Preferences.Default.Get(key, defaultValue);
    }

    public void Set(string key, string value)
    {
        Preferences.Default.Set(key, value);
    }

    public void Remove(string key)
    {
        Preferences.Default.Remove(key);
    }

    public bool ContainsKey(string key)
    {
        return Preferences.Default.ContainsKey(key);
    }
}
