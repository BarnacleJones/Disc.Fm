using System.Text.Json.Serialization;

namespace Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

public class MusicBrainzArtist
{
    [JsonPropertyName("begin-area")]
    public BeginArea BeginArea { get; set; }

    [JsonPropertyName("gender")]
    public object Gender { get; set; }

    [JsonPropertyName("end-area")]
    public EndArea EndArea { get; set; }

    [JsonPropertyName("isnis")]
    public List<string> Isnis { get; set; }

    [JsonPropertyName("aliases")]
    public List<object> Aliases { get; set; }

    [JsonPropertyName("release-groups")]
    public List<ReleaseGroup> ReleaseGroups { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("life-span")]
    public LifeSpan LifeSpan { get; set; }

    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }

    [JsonPropertyName("gender-id")]
    public object GenderId { get; set; }

    [JsonPropertyName("area")]
    public Area Area { get; set; }

    [JsonPropertyName("releases")]
    public List<Release> Releases { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("ipis")]
    public List<object> Ipis { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }
}

public class BeginArea
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("life-span")]
    public LifeSpan LifeSpan { get; set; }
}

public class BeginArea2
{
    [JsonPropertyName("type-id")]
    public object TypeId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("type")]
    public object Type { get; set; }
}
