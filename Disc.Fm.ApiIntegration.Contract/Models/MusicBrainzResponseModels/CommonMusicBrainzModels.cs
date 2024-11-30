using System.Text.Json.Serialization;

namespace Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

public class Area
{
    [JsonPropertyName("type-id")]
    public object TypeId { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("iso-3166-1-codes")]
    public List<string>? Iso31661Codes { get; set; }

    [JsonPropertyName("life-span")]
    public LifeSpan? LifeSpan { get; set; }
}

public class Artist
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }

    [JsonPropertyName("score")]
    public int? Score { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("area")]
    public Area Area { get; set; }

    [JsonPropertyName("begin-area")]
    public BeginArea BeginArea { get; set; }

    [JsonPropertyName("isnis")]
    public List<string> Isnis { get; set; }

    [JsonPropertyName("life-span")]
    public LifeSpan? LifeSpan { get; set; }

    [JsonPropertyName("aliases")]
    public List<Alias> Aliases { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("gender-id")]
    public string GenderId { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("ipis")]
    public List<string> Ipis { get; set; }

    [JsonPropertyName("end-area")]
    public EndArea EndArea { get; set; }
}

public class ReleaseGroup
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }

    [JsonPropertyName("score")]
    public int? Score { get; set; }

    [JsonPropertyName("primary-type-id")]
    public string PrimaryTypeId { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("first-release-date")]
    public string FirstReleaseDate { get; set; }

    [JsonPropertyName("primary-type")]
    public string PrimaryType { get; set; }

    [JsonPropertyName("secondary-types")]
    public List<string> SecondaryTypes { get; set; }

    [JsonPropertyName("secondary-type-ids")]
    public List<string> SecondaryTypeIds { get; set; }

    [JsonPropertyName("artist-credit")]
    public List<ArtistCredit> ArtistCredit { get; set; }

    [JsonPropertyName("releases")]
    public List<Release> Releases { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }
}

public class EndArea
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

public class Alias
{
    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("locale")]
    public string Locale { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("primary")]
    public bool? Primary { get; set; }

    [JsonPropertyName("begin-date")]
    public string BeginDate { get; set; }

    [JsonPropertyName("end-date")]
    public string EndDate { get; set; }

    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }
}

public class LifeSpan
{
    [JsonPropertyName("ended")]
    public bool? Ended { get; set; }

    [JsonPropertyName("end")]
    public string? End { get; set; }

    [JsonPropertyName("begin")]
    public string? Begin { get; set; }
}

public class TextRepresentation
{
    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("script")]
    public string Script { get; set; }
}

public class ReleaseEvent
{
    [JsonPropertyName("area")]
    public Area Area { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }
}

public class ArtistCredit
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artist")]
    public Artist Artist { get; set; }

    [JsonPropertyName("joinphrase")]
    public string Joinphrase { get; set; }
}

//get a lot more populated on release once you have musicbrianz artist id
public class Release
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("barcode")]
    public string Barcode { get; set; }

    [JsonPropertyName("aliases")]
    public List<object> Aliases { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("status-id")]
    public string StatusId { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("release-events")]
    public List<ReleaseEvent> ReleaseEvents { get; set; }

    [JsonPropertyName("quality")]
    public string Quality { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("packaging")]
    public string Packaging { get; set; }

    [JsonPropertyName("text-representation")]
    public TextRepresentation TextRepresentation { get; set; }

    [JsonPropertyName("packaging-id")]
    public string PackagingId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }
}