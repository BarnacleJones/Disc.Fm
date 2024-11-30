using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Models;
using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class InsightsDataService : IInsightsDataService
{
    private readonly ISQLiteAsyncConnection _db;

    public InsightsDataService(ISQLiteAsyncConnection db)
    {
        _db = db;
    }

    public async Task<List<CollectionStatisticData>> GetCollectionStatisticData()
    {
        var collectionDataQuery = @$"SELECT HasAllApiData, DateAdded FROM Release;";
        var collectionData = await _db.QueryAsync<CollectionStatisticData>(collectionDataQuery);

        return collectionData;
    }

    public async Task<List<ReleaseStatisticData>> GetReleaseStatisticData()
    {
        var collectionDataQuery = @$"SELECT
                                         OriginalReleaseYear,
                                         DateAdded,
                                         ReleaseCountry,
                                         Title,
                                         Year
                                         FROM Release;";

        var collectionData = await _db.QueryAsync<ReleaseStatisticData>(collectionDataQuery);

        return collectionData;
    }

    #region Track Insight Data Unused
    public async Task<TracksInsightDataModel> GetTrackInsightData()
    {
        //No point in these metrics. The number of releases actually getting musicbrainz track data and lengths is low
        //Keeping code bones for when the idea of what to do here comes


        //refactor this to return just the needed track data. use a query
        //var tracks = await _db.Table<Track>().ToListAsync();


        //var averageTrackLengthFormatted = GetAverageTrackLengthStringFormatted(tracks);
        //var averageTracksPerReleaseText = GetAverageTracksPerReleaseStringFormatted(tracks);

        return new TracksInsightDataModel
        {
            //AverageTrackLength = averageTrackLengthFormatted,//in progress dont use entire table to do this calculation
            //AverageTracksPerRelease = averageTracksPerReleaseText//see above,
            //another property to indicate how many releases/tracks have musicbrainz data - as the averages are pretty loose figures
        };
    }


    //No point in these metrics. The number of releases actually getting musicbrainz track data and lengths is low
    //Keeping code bones for when the idea of what to do here comes

    //private static string GetAverageTracksPerReleaseStringFormatted(List<Track> tracks)
    //{
    //    var releasesToTracks = tracks.GroupBy(x => x.DiscogsReleaseId).ToList();

    //    var tracksPerReleaseCount = new List<int>();
    //    foreach (var release in releasesToTracks)
    //    {
    //        tracksPerReleaseCount.Add(release.Count());
    //    }
    //    var averageTracksPerReleaseText = Math.Round(tracksPerReleaseCount.Average(), 0, MidpointRounding.AwayFromZero).ToString();
    //    return averageTracksPerReleaseText;
    //}

    //private static string GetAverageTrackLengthStringFormatted(List<Track> tracks)
    //{
    //    //discogs release tracks are strings - so only getting 

    //    var unformattedAverageTrackLength = tracks
    //                                        .Where(x => x.MusicBrainzTrackLength != null)
    //                                        .Average(x => x.MusicBrainzTrackLength);
    //    var averageTrackLengthFormatted = "";
    //    if (unformattedAverageTrackLength.HasValue)
    //    {
    //        averageTrackLengthFormatted = $"{TimeSpan.FromMilliseconds(unformattedAverageTrackLength.Value).ToString(@"mm\:ss")}";
    //    }

    //    return averageTrackLengthFormatted;
    //}

    #endregion
}
