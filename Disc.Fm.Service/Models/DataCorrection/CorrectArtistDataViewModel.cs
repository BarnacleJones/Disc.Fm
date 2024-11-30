namespace Disc.Fm.Service.Models.DataCorrection;

public class CorrectArtistDataViewModel
{
    public string CorrectArtistMusicBrainzId { get; set; }
    public string ArtistName { get; set; }
    public string Country { get; set; }
    public string Disambiguation { get; set; }

    public override string ToString()
    {
        return $"{ArtistName} - {Country} - {Disambiguation}";
    }
}
