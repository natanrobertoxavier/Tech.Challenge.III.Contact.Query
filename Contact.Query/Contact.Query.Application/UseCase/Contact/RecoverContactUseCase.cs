using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication;
using Contact.Query.Communication.Response;
using Contact.Query.Exceptions;
using Contact.Query.Exceptions.ExceptionBase;
using Contact.Query.Communication.Request;
using Contact.Query.Domain.Repositories.Contact;
using Contact.Query.Domain.Repositories.Factories;

namespace Contact.Query.Application.UseCase.Contact;
public class RecoverContactUseCase(
    IContactReadOnlyRepository contactReadOnlyRepository,
    IRegionQueryServiceApiFactory repositoryFactory) : IRecoverContactUseCase
{
    private readonly IContactReadOnlyRepository _contactReadOnlyRepository = contactReadOnlyRepository;
    private readonly IRegionQueryServiceApiFactory _repositoryFactory = repositoryFactory;

    public async Task<Result<ResponseListContactJson>> RecoverAllAsync()
    {
        var entities = await _contactReadOnlyRepository.RecoverAllAsync();

        //return await MapToResponseContactJson(entities);
        throw new NotImplementedException();
    }

    public async Task<Result<ResponseListContactJson>> RecoverListAsync(RegionRequestEnum region)
    {
        var dddIds = await RecoverDDDIdsByRegion(region.GetDescription());

        var entities = await _contactReadOnlyRepository.RecoverAllByDDDIdAsync(dddIds);

        //return await MapToResponseContactJson(entities);
        throw new NotImplementedException();
    }

    public async Task<Result<ResponseListContactJson>> RecoverListByDDDAsync(int ddd)
    {
        var regionIds = await RecoverRegionIdByDDD(ddd);

        var entities = await _contactReadOnlyRepository.RecoverByDDDIdAsync(regionIds);

        //if (entities is not null &&
        //    entities.Any())
        //    return await MapToResponseContactJson(entities);

        //return new List<ResponseContactJson>();
        throw new NotImplementedException();
    }


    public Task<Result<ResponseListContactJson>> RecoverByIdsAsync(RequestListIdJson ids)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ResponseThereIsContactJson>> ThereIsContactAsync(int ddd, string phoneNumber)
    {
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<ResponseContactJson>> MapToResponseContactJson(IEnumerable<Domain.Entities.Contact> entities)
    {
        var tasks = entities.Select(async entity =>
        {
            var (regionReadOnlyrepository, scope) = _repositoryFactory.Create();

            using (scope)
            {
                var ddd = await regionReadOnlyrepository.RecoverByIdAsync(entity.DDDId);

                //return new ResponseContactJson
                //{
                //    ContactId = entity.Id,
                //    FirstName = entity.FirstName,
                //    LastName = entity.LastName,
                //    DDD = ddd.DDD,
                //    Region = ddd.Region,
                //    Email = entity.Email,
                //    PhoneNumber = entity.PhoneNumber
                //};
            }
        });

        //var responseContactJson = await Task.WhenAll(tasks);
        //return responseContactJson;
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<Guid>> RecoverDDDIdsByRegion(string region)
    {
        var (regionReadOnlyRepository, scope) = _repositoryFactory.Create();

        using (scope)
        {
            //var ddd = await regionReadOnlyRepository.RecoverListDDDByRegionAsync(region);

            //return ddd.Select(ddd => ddd.Id).ToList();
        }
        throw new NotImplementedException();
    }

    private async Task<Guid> RecoverRegionIdByDDD(int ddd)
    {
        var (regionReadOnlyRepository, scope) = _repositoryFactory.Create();

        using (scope)
        {
            //var regionDDD = await regionReadOnlyRepository.RecoverByDDDAsync(ddd) ??
            //    throw new ValidationErrorsException(new List<string>() { ErrorsMessages.DDDNotFound });

            //return regionDDD.Id;
        }
        throw new NotImplementedException();
    }
}
