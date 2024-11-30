using Disc.Fm.DataAccess.Contract.Database;
using SQLite;

namespace Disc.Fm.DataAccess.Contract.Entities;

public class DiscogsGenreTags : IDatabaseEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? DiscogsTag { get; set; }
}
