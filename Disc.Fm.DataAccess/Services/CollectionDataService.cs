using Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels;
using Disc.Fm.ApiIntegration.Contract.Services;
using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Entities;
using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class CollectionDataService : ICollectionDataService
{
    private readonly ISQLiteAsyncConnection _db;
    private readonly IDiscogsApiService _discogsApiService;

    public CollectionDataService(ISQLiteAsyncConnection db, IDiscogsApiService discogsApiService)
    {
        _db = db;
        _discogsApiService = discogsApiService;
    }
    public async Task<List<SimpleReleaseData>> GetSimpleReleaseDataForWholeCollection()
    {
        var query = $@"
                        SELECT
                        Artist.Name,
                        Release.DiscogsArtistId,
                        Release.DiscogsReleaseId,
                        Release.Year,
                        Release.Title,
                        Release.DateAdded
                        FROM Release
                        INNER JOIN Artist on Release.DiscogsArtistId = Artist.DiscogsArtistId;";
        var data = await _db.QueryAsync<SimpleReleaseData>(query);
        
        return data;

    }
     public async Task<List<SimpleReleaseData>> GetSimpleReleaseDataForCollectionDataWithoutAllApiData()
    {
        var query = $@"
                        SELECT
                        Artist.Name,
                        Release.DiscogsArtistId,
                        Release.DiscogsReleaseId,
                        Release.Year,
                        Release.Title,
                        Release.DateAdded
                        FROM Release
                        INNER JOIN Artist on Release.DiscogsArtistId = Artist.DiscogsArtistId
                        WHERE Release.HasAllApiData = 0;";

        var data = await _db.QueryAsync<SimpleReleaseData>(query);
        
        return data;

    }

    public async Task<bool> UpdateCollectionFromDiscogs()
    {
        return await MakeCollectionApiCallAndSaveDiscogsCollectionResponse();
    }
    public async Task<bool> CheckCollectionIsSeededOrSeed()
    {
        var oneRecordQuery = @$"SELECT DiscogsReleaseId FROM Release LIMIT 1;";
        var oneRecord = await _db.QueryAsync<DiscogsReleaseIdClass>(oneRecordQuery);

        if (oneRecord == null || (oneRecord != null && oneRecord.Select(x => x.DiscogsReleaseId).FirstOrDefault() == 0))
        {
            return await MakeCollectionApiCallAndSaveDiscogsCollectionResponse();
        }
        return true;
    }
    private async Task<bool> MakeCollectionApiCallAndSaveDiscogsCollectionResponse()
    {
        var data = await _discogsApiService.GetCollectionFromDiscogsApi();
        if (data != null)
        {
            return await SaveDiscogsCollectionResponseAndRemoveDataNotInCollection(data);
        }
        return false;
    }

    #region Saving Collection from API Reponse
    private async Task<bool> SaveDiscogsCollectionResponseAndRemoveDataNotInCollection(DiscogsCollectionResponse collectionResponse)
    {
        await SaveCollectionDataFromDiscogsCollectionResponse(collectionResponse);
        await RemoveDataNoLongerInCollection(collectionResponse);

        return true;
    }

    private async Task SaveCollectionDataFromDiscogsCollectionResponse(DiscogsCollectionResponse collectionResponse)
    {
        if (collectionResponse.releases != null && collectionResponse.releases.Count > 0)
        {
            var discogsArtistIdToArtistToInsert = new List<Artist>();
            var discogsArtistIdToReleaseToInsert = new List<Release>();
            var genreTagsToInsert = new List<DiscogsGenreTags>();

            var discogsGenreTagToDiscogsReleaseDataToInsert = new List<TempGenreStorageHelper>();


            var existingArtistIds = await _db.AllDiscogsArtistIdsInDb();
            var existingReleaseIds = await _db.AllDiscogsReleaseIdsInDb();
            var existingGenreTags = await _db.AllDiscogsGenreTagsInDb();

            var apiReleases = collectionResponse.releases.Where(x => x.id.HasValue).ToList();

            foreach (var release in apiReleases)
            {
                if (!existingReleaseIds.Contains(release.id.Value))
                {
                    var artistDataForThisRelease = release.basic_information?.artists?.FirstOrDefault();//only will save first artist for release, even though there may be many

                    //Add Release Data
                    discogsArtistIdToReleaseToInsert.Add(new Release
                    {
                        DiscogsArtistId = artistDataForThisRelease?.id ?? 0,
                        DiscogsReleaseId = release.id,//this id is the same as basicinformation.id
                        DiscogsMasterId = release.basic_information.master_id,
                        Title = release.basic_information.title,
                        Year = release.basic_information.year,
                        DateAdded = release.date_added
                    });
                    existingReleaseIds.Add(release.id.Value);

                    //Add Artist Data
                    if (artistDataForThisRelease.id.HasValue && !existingArtistIds.Contains(artistDataForThisRelease.id.Value))
                    {
                        discogsArtistIdToArtistToInsert.Add(new Artist
                        {
                            DiscogsArtistId = artistDataForThisRelease.id.Value,
                            Name = artistDataForThisRelease.name,
                        });

                        existingArtistIds.Add(artistDataForThisRelease.id.Value);//wont error if already in hashset
                    }

                    var releaseGenresFromReleaseResponse = release.basic_information.genres;

                    //Add Genre Data
                    foreach (var genre in releaseGenresFromReleaseResponse)
                    {
                        if (string.IsNullOrEmpty(genre))
                            continue;

                        if (!existingGenreTags.Contains(genre))
                        {
                            genreTagsToInsert.Add(new DiscogsGenreTags { DiscogsTag = genre });
                            existingGenreTags.Add(genre);
                        }

                        discogsGenreTagToDiscogsReleaseDataToInsert.Add(new TempGenreStorageHelper
                        {
                            DiscogsReleaseId = release.id.Value,
                            DiscogsArtistId = artistDataForThisRelease?.id ?? 0,
                            DiscogsTag = genre
                        });
                    }
                }
            }

            //insert all new genres
            await _db.InsertAllAsync<DiscogsGenreTags>(genreTagsToInsert);
            await _db.InsertAllAsync<Artist>(discogsArtistIdToArtistToInsert);
            await _db.InsertAllAsync<Release>(discogsArtistIdToReleaseToInsert);

            //Add DiscogsGenreTagToDiscogsRelease Data
            var updatedGenreTags = await _db.Table<DiscogsGenreTags>().ToListAsync();

            var genreTagIdMap = updatedGenreTags.ToDictionary(tag => tag.DiscogsTag, tag => tag.Id);

            var genreTagToReleaseInsertList = discogsGenreTagToDiscogsReleaseDataToInsert.Select(g => new DiscogsGenreTagToDiscogsRelease
            {
                DiscogsArtistId = g.DiscogsArtistId,
                DiscogsGenreTagId = genreTagIdMap[g.DiscogsTag],
                DiscogsReleaseId = g.DiscogsReleaseId,
            }).ToList();

            await _db.InsertAllAsync<DiscogsGenreTagToDiscogsRelease>(genreTagToReleaseInsertList);
        }
    }

    private async Task RemoveDataNoLongerInCollection(DiscogsCollectionResponse collectionResponse)
    {
        var artistsInResponse = collectionResponse.releases
           .SelectMany(r => r.basic_information.artists.Select(a => a.id))
           .ToList();

        await RemoveArtistsNoLongerInCollection(artistsInResponse);

        var releaseIdsFromResponse = collectionResponse.releases.Where(x => x.id.HasValue).Select(x => x.id).ToList();

        await RemoveReleasesNoLongerInCollection(releaseIdsFromResponse);
    }

    private async Task RemoveReleasesNoLongerInCollection(List<int?> releaseIdsFromCollectionResponse)
    {
        var releaseTable = await _db.Table<Release>().ToListAsync();

        // Identify releases in the database that are not in the response
        var releasesToRemove = releaseTable.Where(r => r.DiscogsReleaseId.HasValue)
            .Where(r => !releaseIdsFromCollectionResponse
            .Contains(r.DiscogsReleaseId.Value))
            .ToList();

        // Remove the identified releases from the database
        foreach (var releaseToRemove in releasesToRemove)
        {
            await _db.DeleteAsync(releaseToRemove);
        }
    }

    private async Task RemoveArtistsNoLongerInCollection(List<int?> artistIdsFromResponse)
    {
        var artistsTable = await _db.Table<Artist>().ToListAsync();
        // Identify artists in the database that are not in the response
        var artistsToRemove = artistsTable.Where(a => !artistIdsFromResponse.Contains(a.DiscogsArtistId)).ToList();

        foreach (var artistToRemove in artistsToRemove)
        {
            await _db.DeleteAsync(artistToRemove);
        }
    }


    public class TempGenreStorageHelper
    {
        public string? DiscogsTag { get; set; }
        public int DiscogsArtistId { get; set; }
        public int DiscogsReleaseId { get; set; }
    }

    #endregion

}
