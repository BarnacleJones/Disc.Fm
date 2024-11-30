using SQLite;

namespace Disc.Fm.DataAccess.Contract.Database;

public interface ISQLiteAsyncConnection
{
    Task CreateTableAsync<T>() where T : new();
    AsyncTableQuery<T> Table<T>() where T : new();
    Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new();
    Task<HashSet<int>> AllDiscogsArtistIdsInDb();
    Task<HashSet<int>> AllDiscogsReleaseIdsInDb();
    Task<HashSet<string>> AllDiscogsGenreTagsInDb();
    Task<T> FindAsync<T>(object pk) where T : new();
    Task<T> GetAsync<T>(object pk) where T : new();
    Task<int> InsertAsync(object obj);
    Task<int> UpdateAsync(object obj);
    Task<int> DeleteAsync(object obj);
    Task<int> ExecuteAsync(string query, params object[] args);
    Task<int> DeleteAllAsync<T>() where T : new();
    Task Purge();
    Task PurgeEntireDb();
    Task<T> ExecuteScalarAsync<T>(string query, params object[] args);
    public Task<int> InsertAllAsync<T>(IEnumerable<T> objects, bool runInTransaction = true) where T : IDatabaseEntity;
}
