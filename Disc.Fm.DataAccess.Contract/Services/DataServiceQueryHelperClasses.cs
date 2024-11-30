namespace Disc.Fm.DataAccess.Contract.Services;

public class DiscogsReleaseIdClass
{
    public int DiscogsReleaseId { get; set; }
}
public class DiscogsTags
{
    public string? DiscogsTag { get; set; }
}
public class DiscogsArtistIdClass
{
    public int DiscogsArtistId { get; set; }
}
public class TagDbResponse
{
    public string? Tag { get; set; }
}
public class MusicBrainzReleaseIdResponse
{
    public string? MusicBrainzReleaseId { get; set; }
}

public class TagNameQueryClass
{
    public string Tag { get; set; }
}
public class SimpleReleaseData
{
    public string? Name { get; set; }
    public int DiscogsArtistId { get; set; }
    public int DiscogsReleaseId { get; set; }
    public int Year { get; set; }
    public string? Title { get; set; }
    public DateTime DateAdded { get; set; }

}
