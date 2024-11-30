using Disc.Fm.DataAccess.Contract.Models;

namespace Disc.Fm.DataAccess.Contract.Services;

public interface IArtistDataCorrectionService
{
    Task<List<PossibleArtistsFromMusicBrainzApi>> GetPossibleArtistsForDataCorrectionFromDiscogsReleaseId(int? discogsReleaseId);
    Task DeleteExistingArtistDataAndUpdateToChosenMusicBrainzArtistFromMusicBrainzId(int? discogsReleaseId, string newAritstMusicBrainzId);
}
