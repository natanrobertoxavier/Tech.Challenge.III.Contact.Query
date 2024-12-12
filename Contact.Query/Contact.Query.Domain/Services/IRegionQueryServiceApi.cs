
namespace Contact.Query.Domain.Services;
public interface IRegionQueryServiceApi
{
    Task<object> RecoverByIdAsync(Guid dDDId);
}
