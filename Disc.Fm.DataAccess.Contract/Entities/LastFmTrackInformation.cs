using Disc.Fm.DataAccess.Contract.Database;
using SQLite;

namespace Disc.Fm.DataAccess.Contract.Entities;

public class LastFmTrackInformation : IDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int? DiscogsReleaseId { get; set; }//Foreign key
    public string? TrackName { get; set; }//According to Last.Fm
    public string? ArtistName { get; set; }//According to Last.Fm
    public string? AlbumName { get; set; }//According to Last.Fm
    public int? Rank { get; set; } //Sequence order
    public double? Duration { get; set; }//Milliseconds
}
