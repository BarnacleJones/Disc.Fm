namespace Disc.Fm.Service.Models.EntityViewModels;

public class ArtistViewModel
{
    public string? Artist { get; set; }
    public string? ArtistDescription { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public List<string> Tags { get; set; }
    public List<SimpleReleaseViewModel>? ReleasesInCollection { get; set; }
    public List<MusicBrainzArtistsReleasesViewModel>? ArtistsReleases { get; set; }
}

public class MusicBrainzArtistsReleasesViewModel
{        
    public string Status { get; set; }
    public List<MusicBrainzReleaseViewModel> Releases { get; set; }
}

public class MusicBrainzReleaseViewModel
{
    public string Title { get; set; }
    public string Year { get; set; }
}
