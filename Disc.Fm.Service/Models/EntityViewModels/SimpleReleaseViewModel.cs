namespace Disc.Fm.Service.Models.EntityViewModels;

public class SimpleReleaseViewModel
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
