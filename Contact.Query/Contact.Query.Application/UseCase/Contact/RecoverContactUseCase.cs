using Contact.Query.Application.Services.LoggedUser;
using Contact.Query.Communication;
using Contact.Query.Communication.Request;
using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication.Response;
using Contact.Query.Domain.Repositories.Contact;
using Contact.Query.Domain.Repositories.Factories;
using Serilog;
using TokenService.Manager.Controller;

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

            return output.Success(new ResponseListContactJson(@return.OrderBy(x => x.RegistrationDate).Skip((page - 1) * pageSize).Take(pageSize)));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    public async Task<Result<ResponseListContactJson>> RecoverListAsync(RegionRequestEnum region, int page, int pageSize)
    {
        var output = new Result<ResponseListContactJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverAllAsync)}.");

            var token = await GenerateToken();

            var dddIds = await RecoverDDDIdsByRegion(region.GetDescription(), token);

            var entities = await _contactReadOnlyRepository.RecoverAllByDDDIdAsync(dddIds);

            var @return = await MapToResponseContactJson(entities, token);

            _logger.Information($"End {nameof(RecoverAllAsync)}.;");

            return output.Success(new ResponseListContactJson(@return.OrderBy(x => x.RegistrationDate).Skip((page - 1) * pageSize).Take(pageSize)));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    public async Task<Result<ResponseListContactJson>> RecoverListByDDDAsync(int ddd, int page, int pageSize)
    {
        var output = new Result<ResponseListContactJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverListByDDDAsync)}.");

            var token = await GenerateToken();

            var regionIds = await RecoverRegionIdByDDD(ddd, token);

            var entities = await _contactReadOnlyRepository.RecoverByDDDIdAsync(regionIds);

            var @return = await MapToResponseContactJson(entities, token);

            _logger.Information($"End {nameof(RecoverListByDDDAsync)}.;");

            return output.Success(new ResponseListContactJson(@return.OrderBy(x => x.RegistrationDate).Skip((page - 1) * pageSize).Take(pageSize)));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    public async Task<Result<ResponseThereIsContactJson>> ThereIsContactAsync(int ddd, string phoneNumber)
    {
        var output = new Result<ResponseThereIsContactJson>();

        try
        {
            _logger.Information($"Start {nameof(ThereIsContactAsync)}.");

            var token = await GenerateToken();

            var dddId = await RecoverRegionIdByDDD(ddd, token);

            var therIsContact = await _contactReadOnlyRepository.ThereIsRegisteredContact(dddId, phoneNumber);

            _logger.Information($"End {nameof(ThereIsContactAsync)}.;");

            return output.Success(new ResponseThereIsContactJson(therIsContact));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    public async Task<Result<ResponseListContactJson>> RecoverContactByIdsAsync(RequestListIdJson request)
    {
        var output = new Result<ResponseListContactJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverContactByIdsAsync)}.");

            var token = await GenerateToken();

            var entities = await _contactReadOnlyRepository.RecoverAllByDDDIdAsync(request.Ids);

            var @return = await MapToResponseContactJson(entities, token);

            _logger.Information($"End {nameof(RecoverContactByIdsAsync)}.;");

            return output.Success(new ResponseListContactJson(@return));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    public async Task<Result<ResponseContactJson>> RecoverContactByIdAsync(Guid contactId)
    {
        var output = new Result<ResponseContactJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverListByDDDAsync)}.");

            var token = await GenerateToken();

            var entity = await _contactReadOnlyRepository.RecoverByContactIdAsync(contactId);

            if (entity is null)
            {
                _logger.Information($"{nameof(RecoverListByDDDAsync)} No information found for ID {contactId}.;");
                return output.Success(null);
            }

            var @return = await MapToResponseContactJson(entity, token);

            _logger.Information($"End {nameof(RecoverListByDDDAsync)}.;");

            return output.Success(
                new ResponseContactJson(
                    @return.ContactId,
                    @return.Region,
                    @return.FirstName,
                    @return.LastName,
                    @return.DDD,
                    @return.PhoneNumber,
                    @return.Email,
                    @return.RegistrationDate
                ));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    private async Task<string> GenerateToken()
    {
        var loggedUser = await _loggedUser.RecoverUser();

        return _tokenController.GenerateToken(loggedUser.Email);
    }

    private async Task<IEnumerable<ResponseContactJson>> MapToResponseContactJson(IEnumerable<Domain.Entities.Contact> entities, string token)
    {
        var semaphore = new SemaphoreSlim(10);
        var tasks = new List<Task<ResponseContactJson>>();

        foreach (var entity in entities)
        {
            await semaphore.WaitAsync();

            var task = Task.Run(async () =>
            {
                try
                {
                    var (regionReadOnlyrepository, scope) = _repositoryFactory.Create();

                    using (scope)
                    {
                        var ddd = await regionReadOnlyrepository.RecoverByIdAsync(entity.DDDId, token);

                        if (!ddd.IsSuccess)
                        {
                            _logger.Error($"An error occurred when calling the Region.Query API. Error: {ddd.Error}");
                            return new ResponseContactJson();
                        }

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
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error processing contact: {ContactId}", entity.Id);
                    return new ResponseContactJson();
                }
                finally
                {
                    semaphore.Release();
                }
            });

            tasks.Add(task);
        }

        var responseContactJson = (await Task.WhenAll(tasks)).Where(result => result != null);
        return responseContactJson;
    }


    private async Task<ResponseContactJson> MapToResponseContactJson(Domain.Entities.Contact entity, string token)
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
    }

    private async Task<IEnumerable<Guid>> RecoverDDDIdsByRegion(string region, string token)
    {
        var (regionReadOnlyRepository, scope) = _repositoryFactory.Create();

        using (scope)
        {
            var ddd = await regionReadOnlyRepository.RecoverListDDDByRegionAsync(region, token);

            if (!ddd.IsSuccess)
                throw new Exception($"An error occurred when calling the Region.Query Api. Error {ddd.Error}.");

            return ddd.Data.RegionsDDD.Select(ddd => ddd.Id).ToList();
        }
    }

    private async Task<Guid> RecoverRegionIdByDDD(int ddd, string token)
    {
        var (regionReadOnlyRepository, scope) = _repositoryFactory.Create();

        using (scope)
        {
            var regionDDD = await regionReadOnlyRepository.RecoverByDDDAsync(ddd, token);

            if (!regionDDD.IsSuccess)
                throw new Exception($"An error occurred when calling the Region.Query Api. Error {regionDDD.Error}.");

            return regionDDD.Data.Id;
        }
    }
}
