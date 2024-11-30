using Disc.Fm.DataAccess.Contract.Database;
using SQLite;

namespace Disc.Fm.DataAccess.Contract.Entities;

public class MusicBrainzArtistToMusicBrainzTags : IDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }        
    public int TagId {  get; set; }
    public string? MusicBrainzArtistId { get; set;}
}
