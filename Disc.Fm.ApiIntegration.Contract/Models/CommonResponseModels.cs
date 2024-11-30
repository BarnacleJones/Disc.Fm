namespace Disc.Fm.ApiIntegration.Contract.Models;

public class ResponseArtist
{
    public string? anv { get; set; }
    public int? id { get; set; }//DiscogsArtistId
    public string? join { get; set; }
    public string? name { get; set; }
    public string? resource_url { get; set; }
    public string? role { get; set; }
    public string? tracks { get; set; }
}
public class ResponseCommunity
{
    public List<ResponseContributor>? contributors { get; set; }
    public string? data_quality { get; set; }
    public int? have { get; set; }
    public ResponseRating? rating { get; set; }
    public string? status { get; set; }
    public ResponseSubmitter? submitter { get; set; }
    public int? want { get; set; }
}
public class ResponseUrls
{
}
public class ResponseFormat
{
    public List<string>? descriptions { get; set; }
    public string? name { get; set; }
    public string? qty { get; set; }
    public string? text { get; set; }
}
public class ResponseMember
{
    public bool? active { get; set; }
    public int? id { get; set; }
    public string? name { get; set; }
    public string? resource_url { get; set; }
}
public class ResponseLabel
{
    public string? catno { get; set; }
    public string? entity_type { get; set; }
    public string? entity_type_name { get; set; }
    public int? id { get; set; }
    public string? name { get; set; }
    public string? resource_url { get; set; }
}
public class ResponseExtraartist
{
    public string? join { get; set; }
    public string? name { get; set; }
    public string? anv { get; set; }
    public string? tracks { get; set; }
    public string? role { get; set; }
    public string? resource_url { get; set; }
    public int? id { get; set; }
}
public class ResponseImage
{
    public int? height { get; set; }
    public string? resource_url { get; set; }
    public string? type { get; set; }
    public string? uri { get; set; }
    public string? uri150 { get; set; }
    public int? width { get; set; }
}
public class ResponseTracklist
{
    public string? duration { get; set; }
    public string? position { get; set; }
    public string? type_ { get; set; }
    public string? title { get; set; }
    public List<ResponseExtraartist>? extraartists { get; set; }
}
public class ResponseVideo
{
    public int? duration { get; set; }
    public string? description { get; set; }
    public bool? embed { get; set; }
    public string? uri { get; set; }
    public string? title { get; set; }
}
public class ResponseSublabel
{
    public string? resource_url { get; set; }
    public int? id { get; set; }
    public string? name { get; set; }
}
public class ResponseCompany
{
    public string? catno { get; set; }
    public string? entity_type { get; set; }
    public string? entity_type_name { get; set; }
    public int? id { get; set; }
    public string? name { get; set; }
    public string? resource_url { get; set; }
}
public class ResponseContributor
{
    public string? resource_url { get; set; }
    public string? username { get; set; }
}
public class ResponseIdentifier
{
    public string? type { get; set; }
    public string? value { get; set; }
}
public class ResponseRating
{
    public double? average { get; set; }
    public int? count { get; set; }
}
public class ResponseSubmitter
{
    public string? resource_url { get; set; }
    public string? username { get; set; }
}
