using System.Text.Json.Serialization;

namespace Disc.Fm.ApiIntegration.Contract.Models.MusicBrainzResponseModels;

public class MusicBrainzCover
{
    [JsonPropertyName("images")]
    public List<Images> Images { get; set; }

    [JsonPropertyName("release")]
    public string Release { get; set; }
}
public class Images
{
    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    [JsonPropertyName("front")]
    public bool? Front { get; set; }

    [JsonPropertyName("back")]
    public bool? Back { get; set; }

    [JsonPropertyName("edit")]
    public int? Edit { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    [JsonPropertyName("approved")]
    public bool? Approved { get; set; }

    // Define Id property as object to accommodate both string and number types
    [JsonPropertyName("id")]
    public object Id { get; set; }

    [JsonPropertyName("thumbnails")]
    public Thumbnails Thumbnails { get; set; }
}

public class Thumbnails
{
    [JsonPropertyName("250")]
    public string _250 { get; set; }

    [JsonPropertyName("500")]
    public string _500 { get; set; }

    [JsonPropertyName("1200")]
    public string _1200 { get; set; }

    [JsonPropertyName("large")]
    public string Large { get; set; }

    [JsonPropertyName("small")]
    public string Small { get; set; }
}
