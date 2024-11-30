using Disc.Fm.DataAccess.Contract.Models;

namespace Disc.Fm.DataAccess.Contract.Services;

public interface IArtistDataService
{
    Task<int?> GetARandomDiscogsArtistId();
    Task<ArtistDataModel?> GetArtistDataModelByDiscogsId(int? discogsArtistId);
}
