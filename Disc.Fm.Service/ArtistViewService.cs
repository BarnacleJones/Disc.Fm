using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.EntityViewModels;
using Disc.Fm.Service.Models.Results;
using Microsoft.Extensions.Logging;

namespace Disc.Fm.Service;

public class ArtistViewService
{
    private readonly IArtistDataService _artistDataService;
    private readonly ILogger<ArtistViewService> _logger;
    public ArtistViewService(IArtistDataService artistDataService, ILogger<ArtistViewService> logger)
    {
        _artistDataService = artistDataService;
        _logger = logger;
    }

    public async Task<ViewResult<ArtistViewModel>> GetRandomArtist()
    {
        try
        {
            var randomArtistId = await _artistDataService.GetARandomDiscogsArtistId();
            return await GetArtist(randomArtistId);
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<ArtistViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<ArtistViewModel>> GetArtist(int? discogsArtistId)
    {
        try
        {
            var artistData = await _artistDataService.GetArtistDataModelByDiscogsId(discogsArtistId);

            var releasesViewModel = artistData?.ArtistReleaseDataModels != null 
                ? artistData.ArtistReleaseDataModels.GroupBy(x => x.Status)
                                                    .Select(x => new MusicBrainzArtistsReleasesViewModel
                                                    {
                                                        Status = x.Key,
                                                        Releases = x.OrderBy(y => y.ReleaseYear)
                                                                    .ThenBy(y => y.MusicBrainzReleaseName)
                                                                    .Select(y => new MusicBrainzReleaseViewModel
                                                                    {
                                                                        Title = y.MusicBrainzReleaseName,
                                                                        Year = y.ReleaseYear
                                                                    }).ToList()
                                                    }).ToList()
                : null;

            var mappedReleaseDataToReleaseViewModel = artistData?.ArtistReleaseInCollectionDataModels != null
                ? artistData.ArtistReleaseInCollectionDataModels.Select(x => new SimpleReleaseViewModel
                                                    {
                                                        Artist = x.Artist,
                                                        CoverImage = x.CoverImage,
                                                        DateAdded = x.DateAdded,
                                                        DiscogsArtistId = discogsArtistId,
                                                        DiscogsReleaseId = x.DiscogsReleaseId,
                                                        DiscogsReleaseUrl = x.DiscogsReleaseUrl,
                                                        IsFavourited = x.IsFavourited,
                                                        OriginalReleaseYear = x.OriginalReleaseYear,
                                                        ReleaseCountry = x.ReleaseCountry,
                                                        ReleaseNotes = x.ReleaseNotes,
                                                        Title = x.Title,
                                                        Year = x.Year
                                                    }).ToList()
                : null;

            var data = new ArtistViewModel
            {
                Artist = artistData.Name,
                ArtistDescription = artistData.Profile,
                City = artistData.City,
                Country = artistData.Country,
                StartYear = artistData.StartYear,
                EndYear = artistData.EndYear,
                Tags = artistData.ArtistTags,
                ArtistsReleases = releasesViewModel,
                ReleasesInCollection = mappedReleaseDataToReleaseViewModel
            };

            return new ViewResult<ArtistViewModel>
            {
                Data = data,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<ArtistViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    private void LogError(Exception ex)
    {
        if (ex != null)
        {
            _logger.LogError(ex.StackTrace);
            _logger.LogError(ex.Message);
        }
    }
}
