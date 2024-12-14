using Contact.Query.Api.Filters;
using Contact.Query.Application.UseCase.Contact;
using Contact.Query.Communication.Request;
using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Query.Api.Controllers.v1;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class ContactController : TechChallengeController
{
    [HttpGet]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverAllContacts(
        [FromServices] IRecoverContactUseCase useCase,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await useCase.RecoverAllAsync(page, pageSize);

        return Ok(result);
    }

    [HttpGet]
    [Route("contacts/by-region")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactsByRegion(
        [FromQuery][Required] RegionRequestEnum region,
        [FromServices] IRecoverContactUseCase useCase,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await useCase.RecoverListAsync(region, page, pageSize);

        return Ok(result);
    }

    [HttpGet]
    [Route("contacts/by-ddd")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactsByDDD(
        [FromQuery][Required] int ddd,
        [FromServices] IRecoverContactUseCase useCase,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await useCase.RecoverListByDDDAsync(ddd, page, pageSize);

        return Ok(result);
    }

    [HttpGet]
    [Route("there-is-contact/{ddd}/{phoneNumber}")]
    [ProducesResponseType(typeof(Result<ResponseThereIsContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactsByDDD(
        [FromRoute] int ddd,
        [FromRoute] string phoneNumber,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.ThereIsContactAsync(ddd, phoneNumber);

        return Ok(result);
    }

    [HttpGet]
    [Route("recover-by-id/{contactId}")]
    [ProducesResponseType(typeof(Result<ResponseContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactById(
        [FromRoute] Guid contactId,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverContactByIdAsync(contactId);

        return Ok(result);
    }

    [HttpPost]
    [Route("ddd-ids")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverByIds(
        [FromBody] RequestListIdJson ids,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverContactByIdsAsync(ids);

        return Ok(result);
    }
}
