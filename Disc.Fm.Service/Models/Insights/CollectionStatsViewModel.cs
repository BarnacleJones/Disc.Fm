namespace Disc.Fm.Service.Models.Insights;

public class CollectionStatsViewModel
{
    public int AlbumsInCollection { get; set; }
    public int AlbumsInCollectionWithoutTracksInfo { get; set; }
    public int AlbumsNewToCollectionThisMonth { get; set; }
    public double[] AlbumsNewLastSixMonths { get; set; }
}
