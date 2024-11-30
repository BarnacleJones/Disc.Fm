using Disc.Fm.DataAccess.Contract.Database;
using SQLite;

namespace Disc.Fm.DataAccess.Contract.Entities;

public class MusicBrainzReleaseToCoverImage : IDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }        
    public string? MusicBrainzReleaseId { get; set;}
    public byte[]? MusicBrainzCoverImage { get; set; }
}
