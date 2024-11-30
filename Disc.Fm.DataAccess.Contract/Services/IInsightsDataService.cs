using Disc.Fm.DataAccess.Contract.Models;

namespace Disc.Fm.DataAccess.Contract.Services;
public interface IInsightsDataService
{
    Task<List<CollectionStatisticData>> GetCollectionStatisticData();
    Task<List<ReleaseStatisticData>> GetReleaseStatisticData();
}

