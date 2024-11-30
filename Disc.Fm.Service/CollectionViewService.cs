using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Collection;
using Disc.Fm.Service.Models.Results;
using Microsoft.Extensions.Logging;

namespace Disc.Fm.Service;

public class CollectionViewService
{
    private readonly ICollectionDataService _collectionDataService;
    private readonly IReleaseDataService _releaseDataService;
    private readonly ILogger<CollectionViewService> _logger;

    public CollectionViewService(ICollectionDataService collectionDataService, IReleaseDataService releaseDataService, ILogger<CollectionViewService> logger)
    {
        _collectionDataService = collectionDataService;
        _releaseDataService = releaseDataService;
        _logger = logger;
    }

    public async Task<ViewResult<bool>> ConfirmDataIsSeededAndSeedIfNot()
    {
        try
        {
            var success = _collectionDataService.CheckCollectionIsSeededOrSeed();
            return new ViewResult<bool>
            {
                Data = true,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<bool>
            {
                Data = false,
                ErrorMessage = ex.Message,
                Success = false
            };
        }

    }

    public async Task<ViewResult<List<SimpleCollectionViewData>>> GetCollection()
    {
        try
        {
            var releaseList = await _collectionDataService.GetSimpleReleaseDataForWholeCollection();

            var viewModel = releaseList.Select(x => new SimpleCollectionViewData
            {
                ArtistName = x.Name,
                DiscogsArtistId = x.DiscogsArtistId,
                DiscogsReleaseId = x.DiscogsReleaseId,
                Year = x.Year.ToString(),
                Title = x.Title,
                DateAdded = x.DateAdded
            }).ToList();

            return new ViewResult<List<SimpleCollectionViewData>>
            {
                Data = viewModel,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<SimpleCollectionViewData>>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    public async Task<ViewResult<List<SimpleCollectionViewData>>> GetUntaggedCollection()
    {
        try
        {
            var releaseList = await _collectionDataService.GetSimpleReleaseDataForCollectionDataWithoutAllApiData();

            var viewModel = releaseList.Select(x => new SimpleCollectionViewData
            {
                ArtistName = x.Name,
                DiscogsArtistId = x.DiscogsArtistId,
                DiscogsReleaseId = x.DiscogsReleaseId,
                Year = x.Year.ToString(),
                Title = x.Title,
                DateAdded = x.DateAdded
            }).ToList();

            return new ViewResult<List<SimpleCollectionViewData>>
            {
                Data = viewModel,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return new ViewResult<List<SimpleCollectionViewData>>
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
