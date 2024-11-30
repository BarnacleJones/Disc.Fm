using Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

namespace Disc.Fm.ApiIntegration.Contract.Services;

public interface IMusicBrainzApiService
{
    Task<MusicBrainzInitialArtist> GetInitialArtistFromMusicBrainzApi(string artistName);
    Task<MusicBrainzArtist> GetArtistFromMusicBrainzApiUsingArtistId(string musicBrainzArtistId);
    Task<MusicBrainzRelease> GetReleaseFromMusicBrainzApiUsingMusicBrainsReleaseId(string musicBrainzReleaseId);
    Task<MusicBrainzReleaseGroup> GetReleaseGroupFromMusicBrainzApiUsingMusicBrainsReleaseId(string musicBrainzReleaseId);
}
