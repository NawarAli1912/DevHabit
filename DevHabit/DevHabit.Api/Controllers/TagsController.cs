using System.Net;
using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Tags;
using DevHabit.Api.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("tags")]
public sealed class TagsController(ApplicationDbContext dbContext, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
    [HttpGet]
    public async Task<ActionResult<TagsCollectionDto>> GetTags()
    {
        List<TagDto> tags = await _dbContext
            .Tags
            .Select(TagQueries.ProjectToDto())
            .ToListAsync();

        return Ok(new TagsCollectionDto
        {
            Data = tags
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(string id)
    {
        TagDto? tag = await _dbContext
            .Tags
            .Where(t => t.Id == id)
            .Select(TagQueries.ProjectToDto())
            .FirstOrDefaultAsync();

        return tag is not null ? Ok(tag) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto request, IValidator<CreateTagDto> validator)
    {
        ValidationResult? validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            ProblemDetails problemDetails =
                _problemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest);

            problemDetails.Extensions.Add("errors", validationResult.ToDictionary());
            return BadRequest(problemDetails);
        }

        Tag tag = request.ToEntity();

        if (await _dbContext.Tags.AnyAsync(t => t.Name == tag.Name))
        {
            return
                Problem(detail: $"The tag '{tag.Name}' already exists",
                    statusCode: StatusCodes.Status409Conflict);
        }

        _dbContext.Tags.Add(tag);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag(string id, UpdateTagDto request)
    {
        Tag? tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

        if (tag is null)
        {
            return NotFound();
        }

        tag.UpdateFromDto(request);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        Tag? tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

        if (tag is null)
        {
            return NotFound();
        }

        _dbContext.Tags.Remove(tag);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
