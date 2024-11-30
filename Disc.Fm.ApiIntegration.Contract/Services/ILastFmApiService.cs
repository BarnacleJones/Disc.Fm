using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace Disc.Fm.ApiIntegration.Contract.Services;

public interface ILastFmApiService
{
    public Task<LastResponse> ScrobbleRelease(Scrobble scrobble);
    public Task<LastResponse> ScrobbleReleases(List<Scrobble> scrobbles);
    public Task<LastAlbumResponseManual> GetAlbumInformation(string artistName, string albumName);
}
