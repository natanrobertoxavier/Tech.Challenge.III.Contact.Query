using Contact.Query.Communication.Request;
using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication.Response;

namespace Contact.Query.Application.UseCase.Contact;
public interface IRecoverContactUseCase
{
    Task<Result<ResponseListContactJson>> RecoverAllAsync(int page, int pageSize);
    Task<Result<ResponseListContactJson>> RecoverListAsync(RegionRequestEnum region, int page, int pageSize);
    Task<Result<ResponseListContactJson>> RecoverListByDDDAsync(int ddd, int page, int pageSize);
    Task<Result<ResponseThereIsContactJson>> ThereIsContactAsync(int ddd, string phoneNumber);
    Task<Result<ResponseListContactJson>> RecoverContactByIdsAsync(RequestListIdJson requesst);
    Task<Result<ResponseContactJson>> RecoverContactByIdAsync(Guid contactId);
}
