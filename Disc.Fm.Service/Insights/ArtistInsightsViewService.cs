using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Insights;
using Disc.Fm.Service.Models.Results;

namespace Disc.Fm.Service.Insights;

public class ArtistInsightsViewService
{
    private readonly IInsightsDataService _insightsDataService;

    public ArtistInsightsViewService(IInsightsDataService insightsDataService)
    {
        _insightsDataService = insightsDataService;
    }


    public async Task<ViewResult<ArtistInsightsStatsModel>> GetArtistStatistics()
    {
        try
        {
            return new ViewResult<ArtistInsightsStatsModel>
            {
                Data = null,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new ViewResult<ArtistInsightsStatsModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

}
