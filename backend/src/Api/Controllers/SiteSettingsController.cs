using Application.Features.SiteSettings.Commands;
using Application.Features.SiteSettings.DTOs;
using Application.Features.SiteSettings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SiteSettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SiteSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SiteSettingDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetSiteSettingsQuery());
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(SiteSettingDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateSiteSettingsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
