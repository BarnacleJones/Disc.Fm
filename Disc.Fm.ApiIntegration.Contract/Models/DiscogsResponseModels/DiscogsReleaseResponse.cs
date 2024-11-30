namespace Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels
{
    public class DiscogsReleaseResponse
    {
        public string? title { get; set; }
        public int? id { get; set; }
        public List<ResponseArtist>? artists { get; set; }
        public string? data_quality { get; set; }
        public string? thumb { get; set; }
        public ResponseCommunity? community { get; set; }
        public List<ResponseCompany>? companies { get; set; }
        public string? country { get; set; }
        public DateTime? date_added { get; set; }
        public DateTime? date_changed { get; set; }
        public int? estimated_weight { get; set; }
        public List<ResponseExtraartist>? extraartists { get; set; }
        public int? format_quantity { get; set; }
        public List<ResponseFormat>? formats { get; set; }
        public List<string>? genres { get; set; }
        public List<ResponseIdentifier>? identifiers { get; set; }
        //public List<Image>? images { get; set; } //no idea where these went in the namespace change
        //public List<Label>? labels { get; set; }
        public double? lowest_price { get; set; }
        public int? master_id { get; set; }
        public string? master_url { get; set; }
        public string? notes { get; set; }
        public int? num_for_sale { get; set; }
        public string? released { get; set; }
        public string? released_formatted { get; set; }
        public string? resource_url { get; set; }
        public List<object>? series { get; set; }
        public string? status { get; set; }
        public List<string>? styles { get; set; }
        public List<ResponseTracklist>? tracklist { get; set; }
        public string? uri { get; set; }
        public List<ResponseVideo>? videos { get; set; }
        public int? year { get; set; }
    }
}
