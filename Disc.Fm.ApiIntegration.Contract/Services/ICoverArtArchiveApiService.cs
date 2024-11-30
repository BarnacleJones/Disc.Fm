using Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

namespace Disc.Fm.ApiIntegration.Contract.Services;

public interface ICoverArtArchiveApiService
{
    Task<MusicBrainzCover> GetCoverResponseByMusicBrainzReleaseId(string musicBrainzReleaseId, bool isAReleaseGroupId);
    Task<byte[]> GetCoverByteArray(string? coverUrl);
}
