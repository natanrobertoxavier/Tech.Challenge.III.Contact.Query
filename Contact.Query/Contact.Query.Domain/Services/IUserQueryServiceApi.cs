using Contact.Query.Domain.ResultServices;

namespace Contact.Query.Domain.Services;
public interface IUserQueryServiceApi
{
    Task<Result<UserResult>> RecoverByEmailAsync(string email);
}
