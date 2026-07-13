using Application.Features.Projects.DTOs;
using Application.Features.Projects.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProjectsQuery());
        return Ok(result);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = 52428800)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromForm] Application.Features.Projects.Commands.CreateProjectCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id }, id);
    }

    [HttpPut("{id}")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = 52428800)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromForm] Application.Features.Projects.Commands.UpdateProjectCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new Application.Features.Projects.Commands.DeleteProjectCommand { Id = id });
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(ProjectDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _mediator.Send(new Application.Features.Projects.Queries.GetProjectBySlug.GetProjectBySlugQuery { Slug = slug });
        return Ok(result);
    }

    [HttpPost("{id}/images")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = 52428800)] // 50MB
    [ProducesResponseType(typeof(ProjectImageDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadImage(Guid id, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is required");

        var result = await _mediator.Send(new Application.Features.Projects.Commands.UploadProjectImage.UploadProjectImageCommand { ProjectId = id, File = file });
        return CreatedAtAction(nameof(GetBySlug), new { slug = "temp" }, result); // Since we don't have GetById, mock action name
    }

    [HttpDelete("{id}/images/{imageId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
    {
        var result = await _mediator.Send(new Application.Features.Projects.Commands.DeleteProjectImage.DeleteProjectImageCommand { ProjectId = id, ImageId = imageId });
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/images/reorder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReorderImages(Guid id, [FromBody] List<Guid> imageIds)
    {
        var result = await _mediator.Send(new Application.Features.Projects.Commands.ReorderProjectImagesCommand { ProjectId = id, ImageIds = imageIds });
        if (!result) return NotFound();

        return NoContent();
    }
}
