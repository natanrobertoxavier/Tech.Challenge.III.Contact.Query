using FluentAssertions;
using System.Net;

namespace Contact.Query.Integration.Tests.Api.Controllers.v1;
public class ContactControllerTests() : BaseTestClient("")
{
    private const string URI_REGION_DDD = "/api/v1/contact";
    private const string validEmail = "natan@email.com";

    [Fact]
    public async Task ContactController_Unauthorized_WhenInvalidToken()
    {
        // Arrange
        var token = string.Empty;

        // Act
        var response = await GetRequest(URI_REGION_DDD, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
