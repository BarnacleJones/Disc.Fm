namespace Disc.Fm.DataAccess.Contract.Models;

public class ReleaseDataModel
{
    public string? Year { get; set; }
    public string? OriginalReleaseYear { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? ReleaseNotes { get; set; }
    public string? ReleaseCountry { get; set; }
    public int? DiscogsArtistId { get; set; }
    public int? DiscogsReleaseId { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public string? DiscogsReleaseUrl { get; set; }
    public DateTime? DateAdded { get; set; }
    public List<TrackDto>? Tracks { get; set; }
    public byte[] CoverImage { get; set; }
    public bool IsFavourited { get; set; }
    public string ReleaseGenre { get; set; }
}

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; }

}
public class TrackDto
{
    public string? Title { get; set; }
    public string? Duration { get; set; }
    public string? Position { get; set; }//Eg A1, A2, B1, etc - sorting property
    public int Rating { get; set; }
    public string? Artist { get; set; }
    public string? Release { get; set; }
    public int DiscogsArtistId { get; set; }
    public int DiscogsReleaseId { get; set; }
}
