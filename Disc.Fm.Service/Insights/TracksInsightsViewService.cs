using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Insights;
using Disc.Fm.Service.Models.Results;

namespace Disc.Fm.Service.Insights;

public class TracksInsightsViewService
{
    private readonly IInsightsDataService _insightsDataService;

    public TracksInsightsViewService(IInsightsDataService trackDataService)
    {
        _insightsDataService = trackDataService;
    }

    public async Task<ViewResult<TracksInsightsStatsViewModel>> GetTracksStatistics()
    {
        try
        {
            //No point in these metrics. The number of releases actually getting musicbrainz track data and lengths is low
            //Keeping code bones for when the idea of what to do here comes


            //var tracks = await _insightsDataService.GetTrackInsightData();

            //var averageTrackLengthString = TimeSpan.FromMilliseconds(tracks.AverageTrackLength).ToString(@"mm\:ss");
            //var averageTacksPerRelease = Math.Round(tracks.AverageTracksPerRelease.Average(), 0, MidpointRounding.AwayFromZero).ToString();

            var data = new TracksInsightsStatsViewModel
            {
                //AverageTrackLength = tracks.AverageTrackLength,
                //AverageTracksPerRelease = tracks.AverageTracksPerRelease
            };
            return new ViewResult<TracksInsightsStatsViewModel>
            {
                Data = data,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new ViewResult<TracksInsightsStatsViewModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

}
