using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using TokenService.Manager.Controller;

namespace Contact.Query.Integration.Tests.Api;
public abstract class BaseTestClient
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory<Program> Factory;
    protected readonly string ControllerUri;
    protected readonly TokenController _tokenController;

    protected BaseTestClient(
        string controllerUri)
    {
        ControllerUri = controllerUri;
        Factory = new CustomWebApplicationFactory<Program>();
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions());
        _tokenController = new TokenController(1000, "YTJVPzlCM3Q1KVkoPGM1PlgqNit8MjR4O3Jba1ZR");
    }

    protected static async Task<T> DeserializeResponse<T>(HttpResponseMessage response) =>
        JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

    protected string GetValidToken(string email) =>
        _tokenController.GenerateToken(email);

    protected async Task<HttpResponseMessage> PostRequest(string uri, object body)
    {
        var jsonString = JsonConvert.SerializeObject(body);

        return await Client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PostRequest(string uri, object body, string token = "")
    {
        AuthorizeRequest(token);

        var jsonString = JsonConvert.SerializeObject(body);

        return await Client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> GetRequest(string uri, string token = "")
    {
        AuthorizeRequest(token);

        return await Client.GetAsync(uri);
    }

    protected async Task<HttpResponseMessage> PutRequest(string uri, object body, string token = "")
    {
        AuthorizeRequest(token);

        var jsonString = JsonConvert.SerializeObject(body);

        return await Client.PutAsync(uri, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> DeleteRequest(string uri, string token = "")
    {
        AuthorizeRequest(token);

        return await Client.DeleteAsync(uri);
    }

    private void AuthorizeRequest(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (Client.DefaultRequestHeaders.Contains("Authorization"))
                Client.DefaultRequestHeaders.Remove("Authorization");

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
