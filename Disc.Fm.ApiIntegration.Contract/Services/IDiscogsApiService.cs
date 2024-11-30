using Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels;

namespace Disc.Fm.ApiIntegration.Contract.Services;

public interface IDiscogsApiService
{
    Task<DiscogsCollectionResponse> GetCollectionFromDiscogsApi();
    Task<DiscogsReleaseResponse> GetReleaseFromDiscogs(int discogsReleaseId);
    Task<DiscogsArtistResponse> GetArtistFromDiscogs(int discogsArtistId);
}
