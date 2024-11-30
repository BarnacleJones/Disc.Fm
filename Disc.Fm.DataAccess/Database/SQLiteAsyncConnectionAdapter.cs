using SQLite;
using Microsoft.Extensions.Logging;
using Disc.Fm.DataAccess.Contract.Entities;
using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.DataAccess.Contract.Database;

namespace Disc.Fm.DataAccess.Database
{
    public class SQLiteAsyncConnectionAdapter : Contract.Database.ISQLiteAsyncConnection
    {
        SQLiteAsyncConnection _connection;
        ILogger<SQLiteAsyncConnectionAdapter> _logger;

        public SQLiteAsyncConnectionAdapter(ILogger<SQLiteAsyncConnectionAdapter> logger)
        {
            _logger = logger;
            var a = FileSystem.AppDataDirectory;//helped stop an error below accessing constants.database path but who knows
            _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            _ = InitializeAsync();
        }




        public async Task Purge()
        {
            try
            {
                await _connection.DeleteAllAsync<Artist>();
                await _connection.DeleteAllAsync<Release>();
                //intentionally leaving other data. Use PurgeEntireDb for the other
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception at Disc.FmDb Purge:{ex.Message} ");
                throw;
            }
        }

        public async Task PurgeEntireDb()
        {
            try
            {
                var a = await _connection.DeleteAllAsync<Artist>();
                var b = await _connection.DeleteAllAsync<DiscogsGenreTags>();
                var c = await _connection.DeleteAllAsync<DiscogsGenreTagToDiscogsRelease>();
                var d = await _connection.DeleteAllAsync<MusicBrainzArtistToMusicBrainzRelease>();
                var e = await _connection.DeleteAllAsync<MusicBrainzArtistToMusicBrainzTags>();
                var f = await _connection.DeleteAllAsync<MusicBrainzReleaseToCoverImage>();
                var g = await _connection.DeleteAllAsync<MusicBrainzTags>();
                var h = await _connection.DeleteAllAsync<Release>();
                var i = await _connection.DeleteAllAsync<Track>();
                var j = await _connection.DeleteAllAsync<LastFmTrackInformation>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception at Disc.FmDb Purge all database data:{ex.Message} ");
                throw;
            }
        }
        private async Task InitializeAsync()
        {
            try
            {
                //if (_connection is not null)//removed at start of big rewrite might need it when things are running again?
                //    return;
                var a = Constants.DatabasePath;//handy for debugging figuring out where the db is

                await _connection.CreateTableAsync<Artist>();
                await _connection.CreateTableAsync<Release>();
                await _connection.CreateTableAsync<Track>();
                await _connection.CreateTableAsync<MusicBrainzTags>();
                await _connection.CreateTableAsync<MusicBrainzArtistToMusicBrainzTags>();
                await _connection.CreateTableAsync<MusicBrainzArtistToMusicBrainzRelease>();
                await _connection.CreateTableAsync<MusicBrainzReleaseToCoverImage>();
                await _connection.CreateTableAsync<DiscogsGenreTagToDiscogsRelease>();
                await _connection.CreateTableAsync<DiscogsGenreTags>();
                await _connection.CreateTableAsync<LastFmTrackInformation>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception at Disc.FmDb Init:{ex.Message} ");
                throw;
            }
        }

        public Task<T> ExecuteScalarAsync<T>(string query, params object[] args)
        {
            return _connection.ExecuteScalarAsync<T>(query, args);
        }

        public Task CreateTableAsync<T>() where T : new()
        {
            return _connection.CreateTableAsync<T>();
        }

        public Task<int> DeleteAllAsync<T>() where T : new()
        {
            return _connection.DeleteAllAsync<T>();
        }

        public Task<int> DeleteAsync(object obj)
        {
            return _connection.DeleteAsync(obj);
        }

        public Task<int> ExecuteAsync(string query, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync<T>(object pk) where T : new()
        {
            return _connection.FindAsync<T>(pk);
        }

        public Task<T> GetAsync<T>(object pk) where T : new()
        {
            return _connection.GetAsync<T>(pk);
        }

        public Task<int> InsertAsync(object obj)
        {
            return _connection.InsertAsync(obj);
        }

        public Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
        {
            return _connection.QueryAsync<T>(query, args);
        }

        public AsyncTableQuery<T> Table<T>() where T : new()
        {
            return _connection.Table<T>();
        }

        public Task<int> UpdateAsync(object obj)
        {
            return _connection.UpdateAsync(obj);
        }

        public async Task<HashSet<int>> AllDiscogsArtistIdsInDb()
        {
            var discogsArtistIdsQuery = @$"SELECT DISTINCT DiscogsArtistId FROM Artist";

            var discogsArtistIdsInterim = await _connection.QueryAsync<DiscogsArtistIdClass>(discogsArtistIdsQuery);

            return discogsArtistIdsInterim.Select(artist => artist.DiscogsArtistId).ToHashSet();
        }

        public async Task<HashSet<int>> AllDiscogsReleaseIdsInDb()
        {
            var discogsReleasesInTheDbQuery = @$"SELECT DISTINCT DiscogsReleaseId FROM Release";

            var artistsIdsInDbAlreadyInterim = await _connection.QueryAsync<DiscogsReleaseIdClass>(discogsReleasesInTheDbQuery);

            return artistsIdsInDbAlreadyInterim.Select(x => x.DiscogsReleaseId).ToHashSet();
        }

        public Task<int> InsertAllAsync<T>(IEnumerable<T> objects, bool runInTransaction = true) where T : IDatabaseEntity
        {
            return _connection.InsertAllAsync(objects, runInTransaction);
        }

        public async Task<HashSet<string>> AllDiscogsGenreTagsInDb()
        {
            var discogsReleasesInTheDbQuery = @$"SELECT DISTINCT DiscogsTag FROM DiscogsGenreTags";

            var artistsIdsInDbAlreadyInterim = await _connection.QueryAsync<DiscogsTags>(discogsReleasesInTheDbQuery);

            return artistsIdsInDbAlreadyInterim.Select(x => x.DiscogsTag).ToHashSet();

        }
    }
}
