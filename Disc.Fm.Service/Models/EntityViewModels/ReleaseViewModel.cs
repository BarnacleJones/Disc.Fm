using Disc.Fm.Service.Models.Collection;

namespace Disc.Fm.Service.Models.EntityViewModels;

public class ReleaseViewModel : SimpleReleaseViewModel
{
    public List<ReleaseGenres>? Genres { get; set; }
    public List<TracksItemViewModel>? Tracks { get; set; }
}
public class ReleaseGenres
{
    public int Id { get; set; }
    public string Name { get; set; }
}
