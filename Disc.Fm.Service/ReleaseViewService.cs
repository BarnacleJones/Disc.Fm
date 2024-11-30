using Disc.Fm.Service.Models.EntityViewModels;
using Disc.Fm.Service.Models.Results;
using Disc.Fm.Service.Models.Collection;
using Disc.Fm.Service.Models.DataCorrection;
using Microsoft.Extensions.Logging;
using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.DataAccess.Contract.Models;

namespace Disc.Fm.Service;

public class ReleaseViewService
{
    private readonly IReleaseDataService _releaseDataService;
    private readonly IArtistDataCorrectionService _artistDataCorrectionService;
    private readonly ILogger<ReleaseViewService> _logger;

    public ReleaseViewService(IReleaseDataService releaseDataService, IArtistDataCorrectionService artistDataCorrectionService, ILogger<ReleaseViewService> logger)
    {
        _releaseDataService = releaseDataService;
        _artistDataCorrectionService = artistDataCorrectionService;
        _logger = logger;
    }

    public async Task<ViewResult<ReleaseViewModel>> GetRelease(int? discogsReleaseId)
    {
        try
        {
            if (discogsReleaseId is null) throw new Exception("discogsReleaseId was null");

            var release = await _releaseDataService.GetReleaseDataModelByDiscogsReleaseId(discogsReleaseId.Value);
            if (release is null) throw new Exception("Release was null");

            return new ViewResult<ReleaseViewModel>
            {
                Data = GetReleaseViewModel(release),
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<ReleaseViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<List<ReleaseViewModel>>> GetNewestReleases()
    {
        try
        {
            var newestNumber = 5; //todo: will update this to a setting value
            var newestFiveReleases = await _releaseDataService.GetNewestReleases(newestNumber);
            var returnedReleases = newestFiveReleases.Select(x => GetReleaseViewModel(x)).ToList();

            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = returnedReleases,
                ErrorMessage = "",
                Success = true
            };

        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<ReleaseViewModel>> GetRandomRelease()
    {
        try
        {
            var release = await _releaseDataService.GetRandomRelease();

            return new ViewResult<ReleaseViewModel>
            {
                Data = GetReleaseViewModel(release),
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<ReleaseViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<List<ReleaseViewModel>>> GetReleasesByDiscogsGenreTagId(int discogsGenreTagId)
    {
        try
        {

            var releaseData = await _releaseDataService.GetReleaseDataModelsByDiscogsGenreTagId(discogsGenreTagId);

            var returnedReleases = new List<ReleaseViewModel>();
            foreach (var release in releaseData)
            {
                if (release is null) continue;
                returnedReleases.Add(GetReleaseViewModel(release));
            }

            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = returnedReleases,
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<List<ReleaseViewModel>>> GetReleasesByYear(int? releaseYear)
    {
        try
        {
            if (releaseYear == null) { throw new ArgumentNullException(nameof(releaseYear)); }
            var releaseDataList = await _releaseDataService.GetAllReleaseDataModelsByYear(releaseYear.Value);

            var listToReturn = releaseDataList.Select(x => GetReleaseViewModel(x)).ToList();

            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = listToReturn,
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<ReleaseViewModel>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<bool> SetFavouriteBooleanOnRelease(bool favourited, int discogsReleaseId)
    {
        await _releaseDataService.SetFavouriteBooleanOnRelease(favourited, discogsReleaseId);
        return true;
    }

    public async Task<ViewResult<string>> ScrobbleRelease(int discogsReleaseId, string artistName, string releaseName)
    {
        try
        {
            var scrobble = await _releaseDataService.ScrobbleRelease(discogsReleaseId, artistName, releaseName);

            return new ViewResult<string>
            {
                Success = true,
                Data = scrobble.ToString(),
            };

        }
        catch (Exception ex)
        {
            return new ViewResult<string>
            {
                Success = false,
                Data = ex.Message,
                ErrorMessage = ex.Message
            };
        }
    }

    private ReleaseViewModel GetReleaseViewModel(ReleaseDataModel release)
    {
        var releaseViewModel = new ReleaseViewModel
        {
            ReleaseCountry = release.ReleaseCountry,
            Artist = release.Artist,
            CoverImage = release.CoverImage,
            DateAdded = release.DateAdded,
            DiscogsArtistId = release.DiscogsArtistId,
            DiscogsReleaseId = release.DiscogsReleaseId,
            DiscogsReleaseUrl = release.DiscogsReleaseUrl,
            IsFavourited = release.IsFavourited,
            OriginalReleaseYear = release.OriginalReleaseYear,
            ReleaseNotes = release.ReleaseNotes,
            Title = release.Title,
            Year = release.Year
        };
        releaseViewModel.Genres = new List<ReleaseGenres>();
        if (release.Genres != null)
        {
            foreach (var genre in release?.Genres)
            {
                releaseViewModel.Genres.Add(new ReleaseGenres
                {
                    Id = genre.Id,
                    Name = genre.Name,
                });
            }
        }

        releaseViewModel.Tracks = new List<TracksItemViewModel>();

        if (release.Tracks != null)
        {
            foreach (var trackDto in release.Tracks)
            {
                releaseViewModel.Tracks.Add(new TracksItemViewModel
                {
                    Artist = trackDto.Artist,
                    DiscogsArtistId = trackDto.DiscogsArtistId,
                    DiscogsReleaseId = trackDto.DiscogsReleaseId,
                    Duration = trackDto.Duration,
                    Position = trackDto.Position,
                    Rating = trackDto.Rating,
                    Release = trackDto.Release,
                    Title = trackDto.Title
                });
            }                
        }
        return releaseViewModel;
    }

    #region DataCorrection
    public async Task<ViewResult<List<CorrectArtistDataViewModel>>> GetPossibleArtistsBasedOnDiscogsReleaseId(int? discogsReleaseId)
    {
        try
        {

            var data = await _artistDataCorrectionService.GetPossibleArtistsForDataCorrectionFromDiscogsReleaseId(discogsReleaseId);

            var viewModel = data.Select(x => new CorrectArtistDataViewModel
            {
                ArtistName = x.ArtistName,
                CorrectArtistMusicBrainzId = x.CorrectArtistMusicBrainzId,
                Country = x.Country,
                Disambiguation = x.Disambiguation,
            }).ToList();

            return new ViewResult<List<CorrectArtistDataViewModel>>
            {
                Data = viewModel,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<CorrectArtistDataViewModel>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }
    public async Task<bool> UpdateArtistWithCorrectMusicBrainzId(int? discogsReleaseId, string musicBrainzId)
    {
        await _artistDataCorrectionService.DeleteExistingArtistDataAndUpdateToChosenMusicBrainzArtistFromMusicBrainzId(discogsReleaseId, musicBrainzId);
        return true;
    }
    public async Task<ViewResult<List<CorrectReleaseDataViewModel>>> GetPossibleReleasesBasedOnDiscogsReleaseId(int? discogsReleaseId)
    {
        try
        {
            var data = await _releaseDataService.GetPossibleReleasesForDataCorrectionFromDiscogsReleaseId(discogsReleaseId);

            var viewModel = data.Select(x => new CorrectReleaseDataViewModel
            {
                Date = x.Date,
                MusicBrainzReleaseId = x.MusicBrainzReleaseId,
                Status = x.Status,
                Title = x.Title
            }).ToList();

            return new ViewResult<List<CorrectReleaseDataViewModel>>
            {
                Data = viewModel,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<CorrectReleaseDataViewModel>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }
    public async Task<bool> UpdateReleaseWithCorrectMusicBrainzId(int? discogsReleaseId, string musicBrainzReleaseId)
    {
        var success = await _releaseDataService.UpdateReleaseToBeNewMusicBrainzReleaseId(discogsReleaseId, musicBrainzReleaseId);
        return success;
    }

    #endregion

    private void LogError(Exception ex)
    {
        if (ex != null) 
        {
            _logger.LogError(ex.StackTrace);
            _logger.LogError(ex.Message);
        }
    }
}
