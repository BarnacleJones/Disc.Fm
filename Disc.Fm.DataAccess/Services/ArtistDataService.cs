using Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;
using Disc.Fm.ApiIntegration.Contract.Services;
using Disc.Fm.DataAccess.Contract.Database;
using Disc.Fm.DataAccess.Contract.Entities;
using Disc.Fm.DataAccess.Contract.Models;
using Disc.Fm.DataAccess.Contract.Services;

namespace Disc.Fm.DataAccess.Services;

public class ArtistDataService : IArtistDataService
{
    private readonly IDiscogsApiService _discogsApiService;
    private readonly ISQLiteAsyncConnection _db;
    private readonly IMusicBrainzApiService _musicBrainzApiService;

    public ArtistDataService(IDiscogsApiService discogsApiService,
        ISQLiteAsyncConnection db,
        IMusicBrainzApiService musicBrainzApiService)
    {
        _db = db;
        _discogsApiService = discogsApiService;
        _musicBrainzApiService = musicBrainzApiService;
    }       

    public async Task<int?> GetARandomDiscogsArtistId()
    {
        var randomArtistIdQuery = @$"SELECT DiscogsArtistId FROM Artist ORDER BY RANDOM() LIMIT 1;";
        var artistId = await _db.QueryAsync<DiscogsArtistIdClass>(randomArtistIdQuery);

        return artistId.FirstOrDefault().DiscogsArtistId;
    }
    
    public async Task<ArtistDataModel?> GetArtistDataModelByDiscogsId(int? discogsArtistId)
    {
        if (discogsArtistId == null) { return new ArtistDataModel { Name = "No Artist Id Supplied" }; }
        
        var existingArtist = await GetArtistByDiscogsIdFromDb(discogsArtistId.Value);

        if (existingArtist == null)
        {
            //dont want to store the artist if not in db already- I dont know how would one get here
            throw new Exception($"Unhandled exception: Artist {discogsArtistId} not in db. It might be a 'various artist' issue, or refresh your database");
        }
        if (existingArtist.Name?.ToLower() == "various")
        {                
            //dont fetch api data - 'Various' 404s for discogs, and causes bad data with MusicBrainz. 
            return new ArtistDataModel
            {
                Name = existingArtist.Name,
                City = existingArtist.City,
                Country = existingArtist.Country,
                EndYear = existingArtist.EndYear,
                MusicBrainzArtistId = existingArtist.MusicBrainzArtistId,
                Profile = existingArtist.Profile,
                StartYear = existingArtist.StartYear,
            };
        }
        if (!existingArtist.HasAllApiData)
        {
            if (string.IsNullOrWhiteSpace(existingArtist.Profile))
            {
                var discogsResult = await _discogsApiService.GetArtistFromDiscogs(discogsArtistId.Value);
                //Add additional properties wanted from Artist Discogs call here...profile is really the only useful one here
                //If there is no profile, this api call will be made every time. But for now that is ok I guess.
                existingArtist.Profile = discogsResult.profile;
            }
            if (existingArtist.MusicBrainzArtistId == null && existingArtist.Name != null)
            {
                var musicBrainzResult = await _musicBrainzApiService.GetInitialArtistFromMusicBrainzApi(existingArtist.Name);
                await SetMusicBrainzArtistDataForSavingAndSaveTagsFromArtistResponse(musicBrainzResult, existingArtist);
            }
            existingArtist.HasAllApiData = true;
            await _db.UpdateAsync(existingArtist);
            existingArtist = await _db.GetAsync<DataAccess.Contract.Entities.Artist>(existingArtist.Id);
        }

        var artistReleaseQuery = $@"SELECT 
                           MusicBrainzReleaseName,
                           Status,
                           ReleaseYear
                           FROM MusicBrainzArtistToMusicBrainzRelease
                           WHERE DiscogsArtistId = ?";

        var artistReleaseDataModels = await _db.QueryAsync<ArtistReleaseDataModel>(artistReleaseQuery, existingArtist.DiscogsArtistId);

        var collectionArtistReleaseQuery = $@"SELECT
                               Release.Year,
                               Release.OriginalReleaseYear,
                               Release.Title,
                               Release.ReleaseNotes,
                               Release.ReleaseCountry,
                               Release.DiscogsArtistId,
                               Release.DiscogsReleaseId,
                               Release.DiscogsReleaseUrl,
                               Release.DateAdded,
                               Release.IsFavourited,
                               Artist.Name as Artist,
                               MusicBrainzReleaseToCoverImage.MusicBrainzCoverImage as CoverImage
                               FROM Release
                               INNER JOIN Artist on Release.DiscogsArtistId = Artist.DiscogsArtistId
                               LEFT JOIN MusicBrainzReleaseToCoverImage on Release.MusicBrainzReleaseId = MusicBrainzReleaseToCoverImage.MusicBrainzReleaseId
                               WHERE Release.DiscogsArtistId = ?;";

        var artistReleaseInCollectionDataModels = await _db.QueryAsync<SimpleReleaseDataModel>(collectionArtistReleaseQuery, existingArtist.DiscogsArtistId);

        var tagsQuery = $@"SELECT Tag
                               FROM MusicBrainzTags
                               INNER JOIN MusicBrainzArtistToMusicBrainzTags on MusicBrainzTags.Id = MusicBrainzArtistToMusicBrainzTags.TagId
                               WHERE MusicBrainzArtistToMusicBrainzTags.MusicBrainzArtistId = ?";

        var tagsDto = await _db.QueryAsync<TagDbResponse>(tagsQuery, existingArtist.MusicBrainzArtistId);            
        var tags = tagsDto.Select(x => x.Tag).ToList();

        return new ArtistDataModel
        {
            MusicBrainzArtistId = existingArtist.MusicBrainzArtistId,
            Name = existingArtist.Name,
            Profile = existingArtist.Profile,
            Country = existingArtist.Country,
            City = existingArtist.City,
            StartYear = existingArtist.StartYear,
            EndYear = existingArtist.EndYear,
            ArtistReleaseDataModels = artistReleaseDataModels,
            ArtistTags = tags,
            ArtistReleaseInCollectionDataModels = artistReleaseInCollectionDataModels
        };            
    }

    private async Task<DataAccess.Contract.Entities.Artist?> GetArtistByDiscogsIdFromDb(int discogsArtistId)//keep this here
    {
        var discogsArtistIdsQuery = @$"SELECT * FROM Artist WHERE DiscogsArtistId = ?";

        var discogsArtistIdsInterim = await _db.QueryAsync<DataAccess.Contract.Entities.Artist>(discogsArtistIdsQuery, discogsArtistId);

        var artist = discogsArtistIdsInterim.FirstOrDefault();
        if (artist == null) return null;

        return artist;
    }

    private async Task SetMusicBrainzArtistDataForSavingAndSaveTagsFromArtistResponse(MusicBrainzInitialArtist artistResponse, DataAccess.Contract.Entities.Artist existingArtist)
    {               
        //**this makes an assumption that can cause bad data**
        //Artists in the response is a list, there are similar named artists in the list
        //It looks like the first in the list is closest match
        //Ability to correct this data is on the release card

        var musicBrainsArtistId = artistResponse.Artists.Select(x => x.Id).FirstOrDefault();

        existingArtist.MusicBrainzArtistId = musicBrainsArtistId;

        var artistArea = artistResponse.Artists
                                                .Where(x => x.Id == musicBrainsArtistId)
                                                .Select(x => x.Area)
                                                .FirstOrDefault();
        var artistBeginArea = artistResponse.Artists
                                                .Where(x => x.Id == musicBrainsArtistId)
                                                .Select(x => x.BeginArea)
                                                .FirstOrDefault();
                
        if (artistBeginArea != null)
        {
            var isACity = artistBeginArea.Type == "City";
            var isACountry = artistBeginArea.Type == "Country";
            if (isACity)
            {
                existingArtist.City = artistBeginArea.Name;
            }
            if (isACountry)
            {
                existingArtist.Country = artistBeginArea.Name;
            }
        }
                
        if (artistArea != null)
        {
            var isACity = artistArea.Type == "City";
            var isACountry = artistArea.Type == "Country";
            if (isACountry)
            {
                existingArtist.Country = artistArea.Name;
            }
            if (isACity)
            {
                existingArtist.City = artistArea.Name;
            }
        }

        existingArtist.StartYear = artistResponse.Artists
                                                .Where(x => x.Id == musicBrainsArtistId)
                                                .Select(x => x.LifeSpan?.Begin)
                                                .FirstOrDefault();

        existingArtist.EndYear = artistResponse.Artists
                                                .Where(x => x.Id == musicBrainsArtistId)
                                                .Select(x => x.LifeSpan?.End)
                                                .FirstOrDefault();

        //save tags - yes it is saving to joining table too before saving the actual artist table information, but its saving writes to the db....
        await SaveTagsByMusicBrainzArtistId(artistResponse, musicBrainsArtistId ?? "");
    }

    private async Task SaveTagsByMusicBrainzArtistId(MusicBrainzInitialArtist artistResponse, string musicBrainzArtistId)
    {

        var artistFromResponse = artistResponse.Artists.Where(x => x.Id == musicBrainzArtistId).FirstOrDefault();
        if (artistFromResponse == null) return; //artist id mismatch potentially 

        var tagsInResponse = artistFromResponse.Tags == null ? new List<Tag>() : artistFromResponse.Tags.Where(x => x.Count >= 1).ToList();

        if (tagsInResponse == null || !tagsInResponse.Any())
            return;
        
        var formattedStringListForQuery = string.Join(", ", tagsInResponse.Select(s => $"'{s}'"));
        var tagsFromResponseAlreadyInDbListQuery = @$"
                            SELECT 
                            Tag
                            FROM MusicBrainzTags
                            WHERE TAG IN ({formattedStringListForQuery});";

        var tagsFromResponseAlreadyInDb = await _db.QueryAsync<TagNameQueryClass>(tagsFromResponseAlreadyInDbListQuery);
        
        var tagsToSave = new List<MusicBrainzTags>();
        //if there are tags in db already then dont save those ones
        if (tagsFromResponseAlreadyInDb != null)
        {
            var tagStringsInDb = tagsFromResponseAlreadyInDb.Select(x => x.Tag).ToList();
            foreach (var responseTag in tagsInResponse)
            {
                if (!tagStringsInDb.Contains(responseTag.Name))
                {
                    var tagToSave = new MusicBrainzTags { Tag = responseTag.Name };
                    tagsToSave.Add(tagToSave);
                }
            }
        }
        //otherwise save them all
        else
        {
            foreach (var responseTag in tagsInResponse)
            {
                var tagToSave = new MusicBrainzTags { Tag = responseTag.Name };
                tagsToSave.Add(tagToSave);
            }
        }
        if (tagsToSave != null && tagsToSave.Count > 0)
            await _db.InsertAllAsync(tagsToSave);            

        var tagsFromResponseStrings = tagsInResponse.Select(x => x.Name).ToList();

        var tagRecordsForThisArtistsTags = await _db.Table<MusicBrainzTags>()
                                        .Where(x => tagsFromResponseStrings.Contains(x.Tag))
                                        .ToListAsync();

        var artistToTagRecordsToSave = new List<MusicBrainzArtistToMusicBrainzTags>();
        foreach (var tag in tagsInResponse)
        {
            var tagId = tagRecordsForThisArtistsTags.Where(x => x.Tag == tag.Name).FirstOrDefault()?.Id;
            if (tagId != null)
            {
                var tagToArtist = new MusicBrainzArtistToMusicBrainzTags { TagId = tagId.Value, MusicBrainzArtistId = musicBrainzArtistId };
                artistToTagRecordsToSave.Add(tagToArtist);
            }
        }
        if (artistToTagRecordsToSave != null && artistToTagRecordsToSave.Count > 0)
        {
            await _db.InsertAllAsync(artistToTagRecordsToSave);
        }
    }       
}
