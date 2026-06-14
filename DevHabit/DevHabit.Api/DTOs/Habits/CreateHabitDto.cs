using DevHabit.Api.Entities;
using FluentValidation;

namespace DevHabit.Api.DTOs.Habits;

public sealed record CreateHabitDto
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get;init; }
    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
}

public sealed class CreateHabitDtoValidator : AbstractValidator<CreateHabitDto>
{
    private static readonly string[] AllowedUnits =
    [
        "minutes", "hours", "steps",
        "km", "cal",
        "pages",
        "books",
        "tasks",
        "sessions"
    ];

    private static readonly string[] AllowedUnitsForBinaryHabits = ["sessions", "tasks"];
    
    public CreateHabitDtoValidator()
    {
        RuleFor(t => t.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");
        
        RuleFor(t => t.Description)
            .MaximumLength(500)
            .When(h => h.Description is not null)
            .WithMessage("Description must not exceed 500 characters");
            
        RuleFor(h => h.Type)
            .IsInEnum()
            .WithMessage("Type must be one of 'HabitType'");
        
        RuleFor(h => h.Frequency.Type)
            .IsInEnum()
            .WithMessage("Frequency must be one of 'Frequency'");
        
        RuleFor(h => h.Frequency.TimesInPeriod)
            .GreaterThan(0)
            .WithMessage("Frequency must be greater than zero");
        
        RuleFor(h => h.Target.Value)
            .GreaterThan(0)
            .WithMessage("Target must be greater than zero");
        
        RuleFor(h => h.Target.Unit)
            .NotEmpty()
            .Must(targetUnit => AllowedUnitsForBinaryHabits.Contains(targetUnit.ToLowerInvariant()))
            .WithMessage($"Target Unit must be one of [{string.Join(",", AllowedUnits)}]");
        
        RuleFor(h => h.EndDate)
            .Must(endDate => endDate is null ||
                             endDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage($"End date must be in the future");

        When(h => h.Milestone is not null, () =>
        {
            RuleFor(h => h.Milestone!.Target)
                .GreaterThan(0)
                .WithMessage("Target must be greater than zero");
        });
            
        
        RuleFor(h => h.Target.Unit)
            .Must((dto, unit) => IsTargetUnitCompatiableWithType(dto.Type, unit))
            .WithMessage($"Target Unit must be one of [{string.Join(",", AllowedUnits)}]");
        
    }

    private bool IsTargetUnitCompatiableWithType(HabitType type, string unit)
    {
        string normalizedUnit = unit.ToLowerInvariant();
        return type switch
        {
            HabitType.Binary => AllowedUnitsForBinaryHabits.Contains(normalizedUnit),
            HabitType.Measurable => AllowedUnits.Contains(normalizedUnit),
            _ => false
        };
    }
}
