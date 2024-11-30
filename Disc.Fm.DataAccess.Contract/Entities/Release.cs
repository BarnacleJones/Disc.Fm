using Disc.Fm.DataAccess.Contract.Database;
using SQLite;

namespace Disc.Fm.DataAccess.Contract.Entities;

public class Release : IDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int? DiscogsArtistId { get; set; }//DiscogssArtistId on Artist - FK
    public int? DiscogsReleaseId { get; set; }
    public int? DiscogsMasterId { get; set; }
    public string? Title { get; set; }
    public DateTime? DateAdded { get; set; }
    public int? Year { get; set; } //this is year of the pressing itself
    public string? OriginalReleaseYear { get; set; }
    public string? DiscogsReleaseUrl { get; set; }
    public string? ReleaseCountry { get; set; }
    public string? ReleaseNotes { get; set; }
    public string? MusicBrainzReleaseId { get; set; }
    public bool IsAReleaseGroupGroupId { get; set; }
    public string? MusicBrainzCoverUrl { get; set; }
    public bool IsFavourited { get; set; }
    public bool HasAllApiData { get; set; }//When musicbrainz info is all fetched
    public bool ArtistHasBeenManuallyCorrected { get; set; }
    public bool ReleaseHasBeenManuallyCorrected { get; set; }
}
