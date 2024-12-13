using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication;
using Contact.Query.Communication.Response;
using Contact.Query.Exceptions;
using Contact.Query.Exceptions.ExceptionBase;
using Contact.Query.Communication.Request;
using Contact.Query.Domain.Repositories.Contact;
using Contact.Query.Domain.Repositories.Factories;
using Contact.Query.Application.Services.LoggedUser;
using TokenService.Manager.Controller;
using Serilog.Core;
using Serilog;
using AutoMapper;

namespace Contact.Query.Application.UseCase.Contact;
public class RecoverContactUseCase(
    IContactReadOnlyRepository contactReadOnlyRepository,
    IRegionQueryServiceApiFactory repositoryFactory,
    ILoggedUser loggedUser,
    TokenController tokenController,
    ILogger logger) : IRecoverContactUseCase
{
    private readonly IContactReadOnlyRepository _contactReadOnlyRepository = contactReadOnlyRepository;
    private readonly IRegionQueryServiceApiFactory _repositoryFactory = repositoryFactory;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly TokenController _tokenController = tokenController;
    private readonly ILogger _logger = logger;

    public async Task<Result<ResponseListContactJson>> RecoverAllAsync(int page, int pageSize)
    {
        var output = new Result<ResponseListContactJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverAllAsync)}.");

            var entities = await _contactReadOnlyRepository.RecoverAllAsync();

            var token = await GenerateToken();

            var @return = await MapToResponseContactJson(entities, token);

            _logger.Information($"End {nameof(RecoverAllAsync)}.;");

            return output.Success( new ResponseListContactJson(@return.OrderBy(x => x.RegistrationDate).Skip((page -1) * pageSize).Take(pageSize)));
        }
        catch (ValidationErrorsException ex)
        {
            var errorMessage = $"There are validations errors: {string.Concat(string.Join(", ", ex.ErrorMessages), ".")}";

            _logger.Error(ex, errorMessage);

            return output.Failure(ex.ErrorMessages);
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
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

    private async Task<string> GenerateToken()
    {
        var loggedUser = await _loggedUser.RecoverUser();

        return _tokenController.GenerateToken(loggedUser.Email);
    }

    private async Task<IEnumerable<ResponseContactJson>> MapToResponseContactJson(IEnumerable<Domain.Entities.Contact> entities, string token)
    {
        var tasks = entities.Select(async entity =>
        {
            var (regionReadOnlyrepository, scope) = _repositoryFactory.Create();

            using (scope)
            {
                var ddd = await regionReadOnlyrepository.RecoverByIdAsync(entity.DDDId, token);

                if (!ddd.IsSuccess)
                    throw new Exception($"An error occurred when calling the Region.Query Api. Error {ddd.Error}.");

                return new ResponseContactJson
                {
                    ContactId = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    DDD = ddd.Data.DDD,
                    Region = ddd.Data.Region,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber,
                    RegistrationDate = entity.RegistrationDate
                };
            }
        });

        var responseContactJson = await Task.WhenAll(tasks);
        return responseContactJson;
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
