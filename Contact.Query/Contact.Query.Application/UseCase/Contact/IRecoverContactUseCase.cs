using Contact.Query.Communication.Request;
using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication.Response;

namespace Contact.Query.Application.UseCase.Contact;
public interface IRecoverContactUseCase
{
    Task<Result<ResponseListContactJson>> RecoverAllAsync();
    Task<Result<ResponseListContactJson>> RecoverListAsync(RegionRequestEnum region);
    Task<Result<ResponseListContactJson>> RecoverListByDDDAsync(int ddd);
    Task<Result<ResponseThereIsContactJson>> ThereIsContactAsync(int ddd, string phoneNumber);
    Task<Result<ResponseListContactJson>> RecoverByIdsAsync(RequestListIdJson ids);
}
