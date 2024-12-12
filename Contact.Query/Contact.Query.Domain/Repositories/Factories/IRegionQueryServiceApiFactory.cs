using Contact.Query.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Query.Domain.Repositories.Factories;
public interface IRegionQueryServiceApiFactory
{
    (IRegionQueryServiceApi regionDDDServiceApi, IServiceScope scope) Create();
}
