﻿using Contact.Query.Api.Filters;
using Contact.Query.Application.UseCase.Contact;
using Contact.Query.Communication.Request;
using Contact.Query.Communication.Request.Enum;
using Contact.Query.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Query.Api.Controllers;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class ContactController : TechChallengeController
{
    [HttpGet]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverAllContacts(
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverAllAsync();
        
        return Ok(result);
    }

    [HttpGet]
    [Route("contacts/by-region")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactsByRegion(
        [FromQuery][Required] RegionRequestEnum region,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverListAsync(region);

        return Ok(result);
    }

    [HttpGet]
    [Route("contacts/by-ddd")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverContactsByDDD(
        [FromQuery][Required] int ddd,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverListByDDDAsync(ddd);

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

    [HttpPost]
    [Route("contacts/by-ids")]
    [ProducesResponseType(typeof(Result<ResponseListContactJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverByIds(
        [FromBody] RequestListIdJson ids,
        [FromServices] IRecoverContactUseCase useCase)
    {
        var result = await useCase.RecoverByIdsAsync(ids);

        return Ok(result);
    }
}