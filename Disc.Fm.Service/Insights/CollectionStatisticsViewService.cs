using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Insights;
using Disc.Fm.Service.Models.Results;

namespace Disc.Fm.Service.Insights;

public class CollectionStatisticsViewService
{
    private readonly IInsightsDataService _insightsDataService;

    public CollectionStatisticsViewService(IInsightsDataService insightsDataService)
    {
        _insightsDataService = insightsDataService;
    }

    public async Task<ViewResult<CollectionStatsViewModel>> GetHomePageStatistics()
    {
        try
        {
            var data = await GetCollectionStatsViewModel();

            return new ViewResult<CollectionStatsViewModel>
            {
                Data = data,
                ErrorMessage = "",
                Success = true
            };

        }
        catch (Exception ex)
        {
            return new ViewResult<CollectionStatsViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    private async Task<CollectionStatsViewModel> GetCollectionStatsViewModel()
    {
        var data = await _insightsDataService.GetCollectionStatisticData();

        var releasesWithoutAllApiData = data.Where(x => !x.HasAllApiData).ToList().Count;

        var month = DateTime.Now.Month;
        var releasesNewThisMonth = data.Where(x => x.DateAdded.HasValue && x.DateAdded.Value.Month == month).Count();

        //get last 6 months albums 
        var lastSixMonths = new List<double>();
        for (int i = 5; i >= 0; i--)
        {
            var thisParticularMonth = DateTime.Today.AddMonths(-i).Month;
            var thisParticularYear = DateTime.Today.AddMonths(-i).Year;
            var newToCollectionThisMonth = data.Where(x => x.DateAdded.Value.Month == thisParticularMonth && x.DateAdded.Value.Year == thisParticularYear).Count();
            lastSixMonths.Add(newToCollectionThisMonth);
        }


        return new CollectionStatsViewModel
        {
            AlbumsInCollection = data.Count(),
            AlbumsInCollectionWithoutTracksInfo = releasesWithoutAllApiData,
            AlbumsNewToCollectionThisMonth = releasesNewThisMonth,
            AlbumsNewLastSixMonths = lastSixMonths.ToArray()
        };
    }
}
