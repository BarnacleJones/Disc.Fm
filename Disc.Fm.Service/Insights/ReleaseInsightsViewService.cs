using Disc.Fm.DataAccess.Contract.Models;
using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Insights;
using Disc.Fm.Service.Models.Results;

namespace Disc.Fm.Service.Insights;

public class ReleaseInsightsViewService
{
    private readonly IInsightsDataService _insightsDataService;
    private readonly ISettingsDataService _settingsDataService;

    public ReleaseInsightsViewService(IInsightsDataService insightsDataService, ISettingsDataService settingsDataService)
    {
        _insightsDataService = insightsDataService;
        _settingsDataService = settingsDataService;
    }

    public async Task<ViewResult<ReleaseInsightsStatsModel>> GetReleaseStatistics()
    {
        try
        { 
            var releases = await _insightsDataService.GetReleaseStatisticData();

            var earliestRelease = releases.Where(x => !string.IsNullOrWhiteSpace(x.OriginalReleaseYear))
                                          .OrderBy(x => x.OriginalReleaseYear)
                                          .FirstOrDefault();

            var earliestReleaseText = $"{earliestRelease?.OriginalReleaseYear} - {earliestRelease?.Title}";

            var releasePressingYears = releases.Where(x => x.Year.HasValue && x.Year.Value > 0)
                                               .OrderBy(x => x.Year)
                                               .Select(x => x.Year)
                                               .ToList();

            var averagePressingYear = releasePressingYears.Average() ?? 0;

            var averagePressingYearText = Math.Round(averagePressingYear, 0, MidpointRounding.AwayFromZero).ToString();


            var releasesOverTimeLineChartSeriesData = GenerateDataForReleasesOverTimeGraph(releases);

            var releasesByYearAndLabelsData = GenerateDataForReleasesByPressingCountry(releases);

            var data = new ReleaseInsightsStatsModel
            {
                ReleasesOverTimeLineChartSeriesData = releasesOverTimeLineChartSeriesData,
                EarliestReleaseYear = earliestReleaseText ?? "",
                AverageReleasePressingYear = averagePressingYearText,
                ReleasesPressedCountryLabels = releasesByYearAndLabelsData.Item1,
                ReleasesPressedCountryValues = releasesByYearAndLabelsData.Item2
            };

            return new ViewResult<ReleaseInsightsStatsModel>
            {
                Data = data,
                ErrorMessage = "",
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new ViewResult<ReleaseInsightsStatsModel>
            {
                Data = null,
                ErrorMessage = ex.Message,
                Success = false
            };
        }
    }

    private (string[], double[]) GenerateDataForReleasesByPressingCountry(List<ReleaseStatisticData>? releases)
    {
        var releasesGroupedByCountryOfRelease = releases
            .GroupBy(x => x.ReleaseCountry)
            .Where(x => x.Key != null)
            .ToList();
        var dataList = new List<(string, double)>();

        foreach (var country in releasesGroupedByCountryOfRelease)
        {
            dataList.Add((country.Key ?? "Unknown", country.Count()));//should no longer get unknown
        }

        var labels = new string[dataList.Count()];
        var countryCounts = new double[dataList.Count()];
        for (int i = 0; i < dataList.Count; i++)
        {
            labels[i] = dataList[i].Item1;
            countryCounts[i] = dataList[i].Item2;
        }

        return (labels, countryCounts);
    }

    private List<(string, double)> GenerateDataForReleasesOverTimeGraph(List<ReleaseStatisticData> releases)
    {
        var releasesOverTimeLineChartSeriesData = new List<(string, double)>();

        var groupedByYearReleases = releases.GroupBy(x => x.DateAdded.Value.Year).ToList().OrderBy(x => x.Key);

        foreach (var year in groupedByYearReleases)
        {
            var yearGroupedByMonth = year.GroupBy(x => x.DateAdded.Value.Month).ToList();
            bool startOfYear = true;
            foreach (var monthGroup in yearGroupedByMonth)
            {
                var monthGroupedByDay = monthGroup.GroupBy(x => x.DateAdded.Value.Day).ToList().OrderBy(x => x.Key);

                foreach (var dayGroup in monthGroupedByDay)
                {
                    var label = "";
                    if (startOfYear)
                    {
                        label = year.Key.ToString();
                        startOfYear = false;
                    }
                    releasesOverTimeLineChartSeriesData.Add((label, dayGroup.Count()));

                }

            }
        }

        var settingForInitialExclusionPeriod = _settingsDataService.GetReleaseAddedOverTimeInitialExclusionPeriodInDays();

        if (int.TryParse(settingForInitialExclusionPeriod, out int result))
        {
            var startYearLabel = releasesOverTimeLineChartSeriesData[0].Item1;
            releasesOverTimeLineChartSeriesData.RemoveRange(0, result);
            releasesOverTimeLineChartSeriesData.Insert(0, (startYearLabel, 0));
        }

        return releasesOverTimeLineChartSeriesData;
    }
}
