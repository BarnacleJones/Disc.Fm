namespace Disc.Fm.DataAccess.Contract.Services;

public interface ICollectionDataService
{
    Task<List<SimpleReleaseData>> GetSimpleReleaseDataForWholeCollection();
    Task<List<SimpleReleaseData>> GetSimpleReleaseDataForCollectionDataWithoutAllApiData();
    Task<bool> UpdateCollectionFromDiscogs();
    Task<bool> CheckCollectionIsSeededOrSeed();
}
