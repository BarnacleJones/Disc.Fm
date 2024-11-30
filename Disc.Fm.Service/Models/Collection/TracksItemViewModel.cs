namespace Disc.Fm.Service.Models.Collection;

public class TracksItemViewModel
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
