using FluentValidation;

namespace DevHabit.Api.DTOs.Tags;

public sealed record CreateTagDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public sealed class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(t => t.Description)
            .MaximumLength(250);
    }
}
