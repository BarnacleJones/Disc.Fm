using Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;
using Disc.Fm.ApiIntegration.Contract.Services;
using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Entities;
using Disc.Fm.DataAccess.Contract.Models;
using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class ArtistDataCorrectionService : IArtistDataCorrectionService
{
    private readonly ISQLiteAsyncConnection _db;
    private readonly IMusicBrainzApiService _musicBrainzApiService;

    public ArtistDataCorrectionService(ISQLiteAsyncConnection db,
        IMusicBrainzApiService musicBrainzApiService)
    {
        _db = db;
        _musicBrainzApiService = musicBrainzApiService;
    }

    public async Task<List<PossibleArtistsFromMusicBrainzApi>> GetPossibleArtistsForDataCorrectionFromDiscogsReleaseId(int? discogsReleaseId)
    {
        //Both of these _db calls could be more efficient but its minimal just grabbing one record two times in the rare instance of correcting data

        var singleReleaseRecordForAnArtist = await _db.Table<DataAccess.Contract.Entities.Release>().Where(x => x.DiscogsReleaseId == discogsReleaseId).FirstOrDefaultAsync();
        if (singleReleaseRecordForAnArtist.DiscogsArtistId == null)
        {
            throw new Exception($"Unhandled Exception: No DiscogsArtistId for artist of the release: {singleReleaseRecordForAnArtist.Title}");
        }
        var singleArtistRecordForThisArtist = await _db.Table<DataAccess.Contract.Entities.Artist>().Where(x => x.DiscogsArtistId == singleReleaseRecordForAnArtist.DiscogsArtistId).FirstOrDefaultAsync();
        
        var musicBrainzApiCall = await _musicBrainzApiService.GetInitialArtistFromMusicBrainzApi(singleArtistRecordForThisArtist.Name);

        if (musicBrainzApiCall != null && musicBrainzApiCall.Artists != null)
        {
            var potentialArtists = musicBrainzApiCall.Artists.Select(x => new PossibleArtistsFromMusicBrainzApi
            {
                ArtistName = x.Name ?? "No Name",
                CorrectArtistMusicBrainzId = x.Id ?? "",
                Country = x.Country ?? "Unknown",
                Disambiguation = x.Disambiguation ?? "Unknown",
            }).ToList();
            return potentialArtists;
        }
        return [];
    }

    public async Task DeleteExistingArtistDataAndUpdateToChosenMusicBrainzArtistFromMusicBrainzId(int? discogsReleaseId, string newArtistMusicBrainzId)
    {
        await DeleteAllBadArtistAndReleaseData(discogsReleaseId, newArtistMusicBrainzId);

        var release = await _db.Table<DataAccess.Contract.Entities.Release>().Where(x => x.DiscogsReleaseId == discogsReleaseId).FirstOrDefaultAsync();
        var discogsArtistFromReleaseId = release.DiscogsArtistId;

        if (newArtistMusicBrainzId == null)
        {
            throw new Exception("Unhandled Exception: newArtistMusicBrainzId was null");
        }

        var artistCallResponse = await _musicBrainzApiService.GetArtistFromMusicBrainzApiUsingArtistId(newArtistMusicBrainzId);

        await UpdateArtistTableWithCorrectedData(newArtistMusicBrainzId, artistCallResponse);

        await UpdateMusicBrainzTablesWithCorrectedData(newArtistMusicBrainzId, discogsArtistFromReleaseId, artistCallResponse);
    }

    private async Task UpdateArtistTableWithCorrectedData(string newAritstMusicBrainzId, MusicBrainzArtist artistCallResponse)
    {
        var recordForThisArtist = await _db.Table<DataAccess.Contract.Entities.Artist>().Where(x => x.MusicBrainzArtistId == newAritstMusicBrainzId).ToListAsync();
       
        var artistBeginArea = artistCallResponse.BeginArea;
        var artistArea = artistCallResponse.Area;
        foreach (var artist in recordForThisArtist)
        {

            if (artistBeginArea != null)
            {
                var isACity = artistBeginArea.Type == "City";
                var isACountry = artistBeginArea.Type == "Country";
                if (isACity)
                {
                    artist.City = artistBeginArea.Name;
                }
                if (isACountry)
                {
                    artist.Country = artistBeginArea.Name;
                }
                else
                {
                    artist.City = artistBeginArea.Name;
                }
            }

            if (artistArea != null)
            {
                var isACity = artistArea.Type == "City";
                var isACountry = artistArea.Type == "Country";
                if (isACountry)
                {
                    artist.Country = artistArea.Name;
                }
                if (isACity)
                {
                    artist.City = artistArea.Name;
                }
                else
                {
                    artist.Country = artistArea.Name;
                }

            }

            artist.StartYear = artistCallResponse.LifeSpan.Begin;
            artist.EndYear = artistCallResponse.LifeSpan.End;

            await _db.UpdateAsync(artist);
        }
    }
    
    private async Task UpdateMusicBrainzTablesWithCorrectedData(string newArtistMusicBrainzId, int? discogsArtistFromReleaseId, MusicBrainzArtist artistCallResponse)
    {
        var releasesByArtist = artistCallResponse.Releases?.ToList();
        var releaseGroupsByArtist = artistCallResponse.ReleaseGroups?.ToList();

        var releasesQuery = $@"SELECT MusicBrainzReleaseId FROM MusicBrainzArtistToMusicBrainzRelease WHERE MusicBrainzArtistId = ?";
        var existingReleasesForThisArtistObjectList = await _db.QueryAsync<MusicBrainzReleaseIdResponse>(releasesQuery, newArtistMusicBrainzId);
        var existingReleasesForThisArtistList = existingReleasesForThisArtistObjectList.Select(x => x.MusicBrainzReleaseId);

        if (releasesByArtist != null && releasesByArtist.Count != 0)
        {
            foreach (var artistsRelease in releasesByArtist)
            {
                if (existingReleasesForThisArtistList.Contains(artistsRelease.Id))
                {
                    continue;//already exists
                }
                var artistIdToReleaseId = new MusicBrainzArtistToMusicBrainzRelease
                {
                    MusicBrainzArtistId = newArtistMusicBrainzId,
                    DiscogsArtistId = discogsArtistFromReleaseId ?? 0,
                    MusicBrainzReleaseId = artistsRelease.Id,
                    MusicBrainzReleaseName = artistsRelease.Title,
                    ReleaseYear = artistsRelease.Date,
                    Status = artistsRelease.Status,
                    IsAReleaseGroupGroupId = false //different cover art endpoint for release group id vs release id
                };
                await _db.InsertAsync(artistIdToReleaseId);
            }
        }
        if (releaseGroupsByArtist != null && releaseGroupsByArtist.Count != 0)
        {
            foreach (var artistsReleaseGroup in releaseGroupsByArtist)
            {
                if (existingReleasesForThisArtistList.Contains(artistsReleaseGroup.Id))
                {
                    continue;//already exists
                }
                var artistIdToReleaseId = new MusicBrainzArtistToMusicBrainzRelease
                {
                    MusicBrainzArtistId = newArtistMusicBrainzId,
                    DiscogsArtistId = discogsArtistFromReleaseId ?? 0,
                    MusicBrainzReleaseId = artistsReleaseGroup.Id,
                    MusicBrainzReleaseName = artistsReleaseGroup.Title,
                    ReleaseYear = artistsReleaseGroup.FirstReleaseDate,
                    Status = artistsReleaseGroup.PrimaryType,
                    IsAReleaseGroupGroupId = true //different cover art endpoint for release group id vs release id
                };
                await _db.InsertAsync(artistIdToReleaseId);
            }
        }
    }

    private async Task DeleteAllBadArtistAndReleaseData(int? discogsReleaseId, string newAritstMusicBrainzId)
    {
        var recordsForThisRelease = await _db.Table<DataAccess.Contract.Entities.Release>().Where(x => x.DiscogsReleaseId == discogsReleaseId).ToListAsync();
        
        foreach (var release in recordsForThisRelease)
        {
            release.MusicBrainzReleaseId = null;
            release.HasAllApiData = false;
            release.ArtistHasBeenManuallyCorrected = true;
            //todo: test if this can be called after the foreach and batch update recordsForThisRelease
            await _db.UpdateAsync(release);
        }

        var artistId = recordsForThisRelease.FirstOrDefault().DiscogsArtistId;
        var recordsForThisArtist = await _db.Table<DataAccess.Contract.Entities.Artist>().Where(x => x.DiscogsArtistId == artistId).ToListAsync();
        
        var badMusicBrainzArtistId = recordsForThisArtist.FirstOrDefault().MusicBrainzArtistId;

        foreach (var artist in recordsForThisArtist)
        {
            artist.MusicBrainzArtistId = newAritstMusicBrainzId;
            artist.City = null;
            artist.Country = null;
            artist.StartYear = null;
            artist.EndYear = null;
            await _db.UpdateAsync(artist);
        }

        var artistsToTagsToRemove = await _db.Table<MusicBrainzArtistToMusicBrainzTags>().Where(x => x.MusicBrainzArtistId == badMusicBrainzArtistId).ToListAsync();
        
        foreach (var artistToTag in artistsToTagsToRemove)
        {
            await _db.DeleteAsync(artistToTag);
        }

        var artistsToReleasesToRemove = await _db.Table<MusicBrainzArtistToMusicBrainzRelease>().Where(x => x.MusicBrainzArtistId == badMusicBrainzArtistId).ToListAsync();
        var listOfReleaseIdsTiedToThisArtist = new List<string>();
        foreach (var artistRecord in artistsToReleasesToRemove)
        {
            listOfReleaseIdsTiedToThisArtist.Add(artistRecord.MusicBrainzReleaseId);
            await _db.DeleteAsync(artistRecord);
        }
        if (listOfReleaseIdsTiedToThisArtist != null && listOfReleaseIdsTiedToThisArtist.Count > 0)
        {
            var coverImagesStoredForBadReleases = await _db.Table<MusicBrainzReleaseToCoverImage>().Where(x => listOfReleaseIdsTiedToThisArtist.Contains(x.MusicBrainzReleaseId)).ToListAsync();

            foreach (var coverImageRecord in coverImagesStoredForBadReleases)
            {
                await _db.DeleteAsync(coverImageRecord);
            }
        }
    }
}
