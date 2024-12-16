using Contact.Query.Domain.Services;
using Microsoft.AspNetCore.Http;
using TokenService.Manager.Controller;

namespace Contact.Query.Application.Services.LoggedUser;
public class LoggedUser(
    IHttpContextAccessor httpContextAccessor,
    TokenController tokenController,
    IUserQueryServiceApi userQueryServiceApi) : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly TokenController _tokenController = tokenController;
    private readonly IUserQueryServiceApi _userQueryServiceApi = userQueryServiceApi;

    public async Task<Domain.Entities.User> RecoverUser()
    {
        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var userEmail = _tokenController.RecoverEmail(token);

        var user = await _userQueryServiceApi.RecoverByEmailAsync(userEmail);

        if (!user.IsSuccess)
        {
            throw new Exception($"An error occurred while authenticating the user by token. Error: {user.Error}");
        }

        return new Domain.Entities.User(user.Data.Id, user.Data.RegistrationDate, user.Data.Name, user.Data.Email, user.Data.Password);
    }
}
