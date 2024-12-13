
using Contact.Query.Domain.ResultServices;

namespace Contact.Query.Domain.Services;
public interface IRegionQueryServiceApi
{
    Task<Result<RegionResult>> RecoverByIdAsync(Guid id, string token);
    Task<Result<ListRegionResult>> RecoverListDDDByRegionAsync(string region, string token);
    Task<Result<RegionResult>> RecoverByDDDAsync(int ddd, string token);
}
