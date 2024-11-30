using Newtonsoft.Json;

namespace Disc.Fm.ApiIntegration.Contract;


public class LastFmTagInner
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class LastFmTag
{
    [JsonProperty("tag")]
    public List<LastFmTagInner> TagList { get; set; }
}

public class Streamable
{
    [JsonProperty("fulltrack")]
    public string Fulltrack { get; set; }

    [JsonProperty("#text")]
    public string Text { get; set; }
}

public class LastFmArtist
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mbid")]
    public string Mbid { get; set; }
}
public class LastFmTrack
{
    [JsonProperty("@attr")]
    public Dictionary<string, string> Attr { get; set; }

    [JsonProperty("duration")]
    public int? Duration { get; set; } // Changed to int? to allow for null values

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("artist")]
    public LastFmArtist Artist { get; set; }
}
public class LastFmImage
{
    [JsonProperty("size")]
    public string Size { get; set; }

    [JsonProperty("#text")]
    public string Text { get; set; }
}

public class Wiki
{
    [JsonProperty("published")]
    public string Published { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}

public class LastFmAlbum
{
    [JsonProperty("artist")]
    public string Artist { get; set; }

    [JsonProperty("mbid")]
    public string Mbid { get; set; }

    [JsonProperty("tags")]
    public LastFmTag Tags { get; set; }

    [JsonProperty("playcount")]
    public string Playcount { get; set; }

    [JsonProperty("image")]
    public List<LastFmImage> Image { get; set; }

    [JsonProperty("tracks")]
    public LastFmTracks Tracks { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("listeners")]
    public string Listeners { get; set; }

    [JsonProperty("wiki")]
    public Wiki Wiki { get; set; }
}

public class LastFmTracks
{
    [JsonProperty("track")]
    public List<LastFmTrack> TrackList { get; set; }
}

public class LastAlbumResponseManual
{
    [JsonProperty("album")]
    public LastFmAlbum Album { get; set; }
}
