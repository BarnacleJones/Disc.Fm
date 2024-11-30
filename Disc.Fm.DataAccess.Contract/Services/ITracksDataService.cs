using Disc.Fm.DataAccess.Contract.Models;

namespace Disc.Fm.DataAccess.Contract.Services;
public interface ITracksDataService
{
    Task<bool> SetRatingOnTrack(int? rating, int discogsReleaseId, string title);
    Task<List<TrackGridModel>> GetAllTracksForGrid();
}

