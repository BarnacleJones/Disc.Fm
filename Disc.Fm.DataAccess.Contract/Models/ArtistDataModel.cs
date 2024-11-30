namespace Disc.Fm.DataAccess.Contract.Models;

public class ArtistDataModel
{
    public string MusicBrainzArtistId { get; set; }
    public string? Name { get; set; }
    public string? Profile { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public List<ArtistReleaseDataModel> ArtistReleaseDataModels { get; set; }
    public List<string> ArtistTags { get; set; }
    public List<SimpleReleaseDataModel> ArtistReleaseInCollectionDataModels { get; set; }
}

public class ArtistReleaseDataModel
{
    public string MusicBrainzReleaseName { get; set; }
    public string Status { get; set; }
    public string ReleaseYear { get; set; }
}

public class SimpleReleaseDataModel
{
    public string? Year { get; set; }
    public string? OriginalReleaseYear { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? ReleaseNotes { get; set; }
    public string? ReleaseCountry { get; set; }
    public int? DiscogsArtistId { get; set; }
    public int? DiscogsReleaseId { get; set; }
    public string? DiscogsReleaseUrl { get; set; }
    public DateTime? DateAdded { get; set; }
    public byte[] CoverImage { get; set; }
    public bool IsFavourited { get; set; }
}
