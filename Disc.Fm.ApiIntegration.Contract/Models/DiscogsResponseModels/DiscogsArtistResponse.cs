namespace Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels;

public class DiscogsArtistResponse
{
    public List<string>? namevariations { get; set; }
    public string? profile { get; set; }
    public string? releases_url { get; set; }
    public string? resource_url { get; set; }
    public string? uri { get; set; }
    public List<string>? urls { get; set; }
    public string? data_quality { get; set; }
    public int? id { get; set; } //discogs artist id foreign key
    public List<ResponseImage>? images { get; set; }
    public List<ResponseMember>? members { get; set; }
}
