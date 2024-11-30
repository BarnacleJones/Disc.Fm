using System.Text.Json.Serialization;

namespace Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

public class MusicBrainzRelease
{
    [JsonPropertyName("release-events")]
    public List<ReleaseEvent> ReleaseEvents { get; set; }

    [JsonPropertyName("cover-art-archive")]
    public CoverArtArchive CoverArtArchive { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("asin")]
    public object Asin { get; set; }

    [JsonPropertyName("status-id")]
    public string StatusId { get; set; }

    [JsonPropertyName("quality")]
    public string Quality { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("media")]
    public List<Medium> Media { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("barcode")]
    public object Barcode { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("packaging")]
    public string Packaging { get; set; }

    [JsonPropertyName("artist-credit")]
    public List<ArtistCredit> ArtistCredit { get; set; }

    [JsonPropertyName("label-info")]
    public List<LabelInfo> LabelInfo { get; set; }

    [JsonPropertyName("text-representation")]
    public TextRepresentation TextRepresentation { get; set; }

    [JsonPropertyName("packaging-id")]
    public string PackagingId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class CoverArtArchive
{
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("darkened")]
    public bool? Darkened { get; set; }

    [JsonPropertyName("back")]
    public bool? Back { get; set; }

    [JsonPropertyName("front")]
    public bool? Front { get; set; }

    [JsonPropertyName("artwork")]
    public bool? Artwork { get; set; }
}

public class Disc
{
    [JsonPropertyName("offsets")]
    public List<int?> Offsets { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sectors")]
    public int? Sectors { get; set; }

    [JsonPropertyName("offset-count")]
    public int? OffsetCount { get; set; }
}

public class Label
{
    [JsonPropertyName("type-id")]
    public string TypeId { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("label-code")]
    public object LabelCode { get; set; }

    [JsonPropertyName("sort-name")]
    public string SortName { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class LabelInfo
{
    [JsonPropertyName("catalog-number")]
    public string CatalogNumber { get; set; }

    [JsonPropertyName("label")]
    public Label Label { get; set; }
}

public class Medium
{
    [JsonPropertyName("track-count")]
    public int? TrackCount { get; set; }

    [JsonPropertyName("tracks")]
    public List<Track> Tracks { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("discs")]
    public List<Disc> Discs { get; set; }

    [JsonPropertyName("format-id")]
    public string FormatId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("track-offset")]
    public int? TrackOffset { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }
}

public class Recording
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("first-release-date")]
    public string FirstReleaseDate { get; set; }

    [JsonPropertyName("length")]
    public int? Length { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("video")]
    public bool? Video { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonPropertyName("artist-credit")]
    public List<ArtistCredit> ArtistCredit { get; set; }
}

public class Track
{
    [JsonPropertyName("length")]
    public int? Length { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("recording")]
    public Recording Recording { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("artist-credit")]
    public List<ArtistCredit> ArtistCredit { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }
}



