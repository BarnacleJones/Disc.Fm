using Disc.Fm.DataAccess.Contract.Services;
using Disc.Fm.Service.Models.Collection;
using Disc.Fm.Service.Models.Results;
using Microsoft.Extensions.Logging;

namespace Disc.Fm.Service
{
    public class TracksViewService
    {
        private readonly ITracksDataService _tracksDataService;
        private readonly ILogger<TracksViewService> _logger;
        public TracksViewService(ITracksDataService tracksDataService, ILogger<TracksViewService> logger)
        {
            _tracksDataService = tracksDataService;
            _logger = logger;
        }

        public async Task<bool> SetRatingOnTrack(int? rating, int discogsReleaseId, string title)
        {
            var success = await _tracksDataService.SetRatingOnTrack(rating, discogsReleaseId, title);
            return success;
        }

        public async Task<ViewResult<TracksGridViewModel>> GetTracksForTracksGrid()
        {
            try
            {
                var tracks = await _tracksDataService.GetAllTracksForGrid();

                var tracksAsGridItems = tracks.Select(x => new TracksItemViewModel
                {
                    DiscogsArtistId = x.DiscogsArtistId ?? 0,
                    DiscogsReleaseId = x.DiscogsReleaseId ?? 0,
                    Duration = x.MusicBrainzTrackLength == null
                                    ? x.Duration
                                    : TimeSpan.FromMilliseconds(x.MusicBrainzTrackLength.Value).ToString(@"mm\:ss"),
                    Title = x.Title,
                    Position = x.Position,
                    Rating = x.Rating ?? 0,
                    Artist = x.ArtistName,
                    Release = x.ReleaseName
                }).ToList();

                return new ViewResult<TracksGridViewModel>
                {
                    Data = new TracksGridViewModel { Tracks = tracksAsGridItems },
                    Success = true
                };
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ViewResult<TracksGridViewModel>
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
}
