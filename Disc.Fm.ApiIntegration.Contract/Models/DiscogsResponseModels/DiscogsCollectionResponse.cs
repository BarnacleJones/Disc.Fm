namespace Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels;

public class DiscogsCollectionResponse
{
    public Pagination? pagination { get; set; }
    public List<ResponseRelease>? releases { get; set; }
}

public class BasicInformation
{
    public List<ResponseArtist>? artists { get; set; }
    public string? cover_image { get; set; }
    public List<ResponseFormat>? formats { get; set; }
    public List<string>? genres { get; set; }
    public int? id { get; set; }
    public List<ResponseLabel>? labels { get; set; }
    public int? master_id { get; set; }
    public string? master_url { get; set; }
    public string? resource_url { get; set; }
    public List<string>? styles { get; set; }
    public string? thumb { get; set; }
    public string? title { get; set; }
    public int? year { get; set; }
}

public class Pagination
{
    public int? items { get; set; }
    public int? page { get; set; }
    public int? pages { get; set; }
    public int? per_page { get; set; }
    public ResponseUrls? urls { get; set; }
}

public class ResponseRelease
{
    public BasicInformation? basic_information { get; set; }
    public DateTime? date_added { get; set; }
    public int? id { get; set; } //discogs release id foreign key
    public int? instance_id { get; set; }
    public int? rating { get; set; }
}
