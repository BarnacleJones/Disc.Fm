namespace Disc.Fm.ApiIntegration.Contract.Models.DiscogsResponseModels
{
    public class DiscogsMasterReleaseResponse
    {
        public List<string>? styles { get; set; }
        public List<string>? genres { get; set; }
        public List<ResponseVideo>? videos { get; set; }
        public string? title { get; set; }
        public int? main_release { get; set; }
        public string? main_release_url { get; set; }
        public string? uri { get; set; }
        public List<ResponseArtist>? artists { get; set; }
        public string? versions_url { get; set; }
        public int? year { get; set; }
        //public List<Image>? images { get; set; } //gotta find where image went!
        public string? resource_url { get; set; }
        public List<ResponseTracklist>? tracklist { get; set; }
        public int? id { get; set; }
        public int? num_for_sale { get; set; }
        public double? lowest_price { get; set; }
        public string? data_quality { get; set; }
    }
}
