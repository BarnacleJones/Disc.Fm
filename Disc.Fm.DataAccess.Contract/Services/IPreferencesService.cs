namespace Disc.Fm.DataAccess.Contract.Services;

public interface IPreferencesService
{
    string Get(string key, string defaultValue);
    void Set(string key, string value);
    void Remove(string key);
    bool ContainsKey(string key);
}

