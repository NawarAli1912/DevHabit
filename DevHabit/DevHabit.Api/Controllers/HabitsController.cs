using System.Linq.Expressions;
using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Habits;
using DevHabit.Api.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits")]
public sealed class HabitsController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<ActionResult<HabitsCollectionDto>> GetHabits()
    {
        List<HabitDto> habitsDto = await _dbContext.Habits.Select(HabitQueries
                .ProjectToDto())
                .ToListAsync();
        return Ok(new HabitsCollectionDto
        {
            Data = habitsDto
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HabitWithTagsDto>> GetHabit(string id)
    {
        HabitWithTagsDto? habit = await _dbContext
            .Habits
            .Where(item => item.Id == id)
            .Select(HabitQueries.ProjectToDtoWithTags())
            .FirstOrDefaultAsync();

        return habit != null ? Ok(habit) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<HabitDto>> CreateHabit(CreateHabitDto request, IValidator<CreateHabitDto> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);
        Habit habit = request.ToEntity();

        if (!validationResult.IsValid)
        {
            ProblemDetails problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                HttpContext, StatusCodes.Status400BadRequest);

            problemDetails.Extensions.Add("errors", validationResult.ToDictionary());

            return BadRequest(problemDetails);
        }

        _dbContext.Habits.Add(habit);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHabit(string id, UpdateHabitDto request)
    {
        Habit? habit = await _dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id);

        if (habit is null)
        {
            return NotFound();
        }

        habit.UpdateFromDto(request);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchHabit(string id, JsonPatchDocument<HabitDto> patchDocument)
    {
        Habit? habit = await _dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id);
        if (habit is null)
        {
            return NotFound();
        }

        HabitDto dto = habit.ToDto();

        patchDocument.ApplyTo(dto, ModelState);

        if (!TryValidateModel(dto))
        {
            return ValidationProblem(ModelState);
        }

        habit.Name = dto.Name;
        habit.Description = dto.Description;
        habit.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit(string id)
    {
        Habit? habit = await _dbContext.Habits.FirstOrDefaultAsync(h => h.Id == id);

        if (habit is null)
        {
            return NotFound();
        }

        _dbContext.Habits.Remove(habit);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
