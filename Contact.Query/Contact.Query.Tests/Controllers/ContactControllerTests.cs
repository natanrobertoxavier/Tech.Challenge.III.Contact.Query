using Contact.Query.Api.Controllers.v1;
using Contact.Query.Application.UseCase.Contact;
using Contact.Query.Communication.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Contact.Query.Tests.Controllers;

public class ContactControllerTests
{
    [Fact]
    public async Task RecoverAllContacts_ShouldReturnOkResult_WithExpectedData()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverContactUseCase>();

        var output = new Result<ResponseListContactJson>();

        var responseList = new List<ResponseContactJson>();

        responseList.Add(new ResponseContactJson()
        {
            ContactId = Guid.NewGuid(),
            DDD = 11,
            Email = "natan@email.com",
            FirstName = "Natan",
            LastName = "Xavier",
            PhoneNumber = "99999-9999",
            Region = "Suldeste",
            RegistrationDate = DateTime.Now,
        });

        var expectedResult = output.Success(new ResponseListContactJson(responseList));

        mockUseCase
            .Setup(x => x.RecoverAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(expectedResult);

        var controller = new ContactController();

        // Act
        var result = await controller.RecoverAllContacts(mockUseCase.Object, page: 1, pageSize: 10) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListContactJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverAllContacts_ShouldReturnOkResult_WithFail()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverContactUseCase>();

        var output = new Result<ResponseListContactJson>();

        var expectedResult = output.Failure(new List<string>() { "Any Error" });

        mockUseCase
            .Setup(x => x.RecoverAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(expectedResult);

        var controller = new ContactController();

        // Act
        var result = await controller.RecoverAllContacts(mockUseCase.Object, page: 1, pageSize: 10) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListContactJson>>(result.Value);

        Assert.NotNull(result);
        Assert.False(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }
}
