namespace Disc.Fm.DataAccess.Contract.Models;

public class ReleaseStatisticData
{
    public string OriginalReleaseYear { get; set; }
    public DateTime? DateAdded { get; set; }
    public string ReleaseCountry { get; set; }
    public string Title { get; set; }
    public int? Year { get; set; }
}
