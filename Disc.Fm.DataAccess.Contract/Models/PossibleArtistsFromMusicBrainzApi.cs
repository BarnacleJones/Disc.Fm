namespace Disc.Fm.DataAccess.Contract.Models;

public class PossibleArtistsFromMusicBrainzApi
{
    public string CorrectArtistMusicBrainzId { get; set; }
    public string ArtistName { get; set; }
    public string Country { get; set; }
    public string Disambiguation { get; set; }
}
