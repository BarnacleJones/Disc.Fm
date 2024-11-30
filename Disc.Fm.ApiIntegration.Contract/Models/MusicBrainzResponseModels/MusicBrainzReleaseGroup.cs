using System.Text.Json.Serialization;

namespace Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

public class MusicBrainzReleaseGroup
{
    [JsonPropertyName("releases")]
    public List<Release> Releases { get; set; }

    [JsonPropertyName("first-release-date")]
    public string FirstReleaseDate { get; set; }

    [JsonPropertyName("secondary-types")]
    public List<string> SecondaryTypes { get; set; }

    [JsonPropertyName("primary-type")]
    public string PrimaryType { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("primary-type-id")]
    public string PrimaryTypeId { get; set; }

    [JsonPropertyName("secondary-type-ids")]
    public List<string> SecondaryTypeIds { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("artist-credit")]
    public List<ArtistCredit> ArtistCredit { get; set; }
}
