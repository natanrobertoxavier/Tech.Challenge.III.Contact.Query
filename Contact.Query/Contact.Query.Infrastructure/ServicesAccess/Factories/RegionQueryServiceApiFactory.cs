using Contact.Query.Domain.Repositories.Factories;
using Contact.Query.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Query.Infrastructure.ServicesAccess.Factories;
public class RegionQueryServiceApiFactory(
    IServiceScopeFactory scopeFactory) : IRegionQueryServiceApiFactory
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;


    public (IRegionQueryServiceApi regionDDDServiceApi, IServiceScope scope) Create()
    {
        var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRegionQueryServiceApi>();
        return (repository, scope);
    }
}
