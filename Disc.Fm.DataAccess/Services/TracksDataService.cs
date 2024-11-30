using Disc.Fm.DataAccess.Contract.Models;
using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.DataAccess.Contract.Entities;

namespace Disc.Fm.DataAccess.Services;

public class TracksDataService : ITracksDataService
{
    private readonly ISQLiteAsyncConnection _db;

    public TracksDataService(ISQLiteAsyncConnection db)
    {
        _db = db;
    }

    public async Task<List<TrackGridModel>> GetAllTracksForGrid()
    {
        var tracksGridModelItemsQuery = $@"SELECT
                                               Track.Id,
                                               Track.DiscogsArtistId,
                                               Track.DiscogsReleaseId,
                                               Track.DiscogsMasterId,
                                               Track.Title,
                                               Track.Duration,
                                               Track.MusicBrainzTrackLength,
                                               Track.Position,
                                               Track.Rating,
                                               Artist.Name as ArtistName,
                                               Release.Title as ReleaseName
                                               FROM Track
                                               INNER JOIN Release on Track.DiscogsReleaseId = Release.DiscogsReleaseId
                                               INNER JOIN Artist on Track.DiscogsArtistId = Artist.DiscogsArtistId;";

        var data = await _db.QueryAsync<TrackGridModel>(tracksGridModelItemsQuery);

        return data;
    }

    public async Task<bool> SetRatingOnTrack(int? rating, int discogsReleaseId, string title)
    {
        var track = await _db.Table<Track>().Where(x => x.DiscogsReleaseId == discogsReleaseId).Where(x => x.Title == title).FirstOrDefaultAsync();
            
        if (track == null) { return true; }

        track.Rating = rating;
        await _db.UpdateAsync(track);

        return true;
    }
}
