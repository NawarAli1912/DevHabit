using System.Linq.Expressions;
using DevHabit.Api.Entities;
using DevHabit.Api.Services.Sorting;

namespace DevHabit.Api.DTOs.Habits;

public sealed class HabitQueries
{
    public static Expression<Func<Habit, HabitDto>> ProjectToDto()
    {
        return item => new HabitDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Type = item.Type,
            Frequency = new FrequencyDto
            {
                Type = item.Frequency.Type,
                TimesInPeriod = item.Frequency.TimesInPeriod
            },
            Target = new TargetDto
            {
                Value = item.Target.Value,
                Unit = item.Target.Unit
            },
            Status = item.Status,
            IsArchived = item.IsArchived,
            EndDate = item.EndDate,
            Milestone = item.Milestone == null
                ? null
                : new MilestoneDto
                {
                    Target = item.Milestone.Target,
                    Current = item.Milestone.Current
                },
            CreatedAtUtc = item.CreatedAtUtc,
            UpdatedAtUtc = item.UpdatedAtUtc,
            LastCompletedAtUtc = item.LastCompletedAtUtc,
        };
    }

    public static Expression<Func<Habit, HabitWithTagsDto>> ProjectToDtoWithTags()
    {
        return item => new HabitWithTagsDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Type = item.Type,
            Frequency = new FrequencyDto
            {
                Type = item.Frequency.Type,
                TimesInPeriod = item.Frequency.TimesInPeriod
            },
            Target = new TargetDto
            {
                Value = item.Target.Value,
                Unit = item.Target.Unit
            },
            Status = item.Status,
            IsArchived = item.IsArchived,
            EndDate = item.EndDate,
            Milestone = item.Milestone == null
                ? null
                : new MilestoneDto
                {
                    Target = item.Milestone.Target,
                    Current = item.Milestone.Current
                },
            CreatedAtUtc = item.CreatedAtUtc,
            UpdatedAtUtc = item.UpdatedAtUtc,
            LastCompletedAtUtc = item.LastCompletedAtUtc,
            Tags = item.Tags.Select(t => t.Name).ToArray()
        };
    }
}
public static class HabitMappings
{
    public static readonly SortMappingDefinition<HabitDto, Habit> SortMapping = new()
    {
        Mappings =
        [
            new SortMapping(nameof(HabitDto.Name), nameof(Habit.Name)),
            new SortMapping(nameof(HabitDto.Description), nameof(Habit.Description)),
            new SortMapping(nameof(HabitDto.Type), nameof(Habit.Type)),
            new SortMapping(
                $"{nameof(HabitDto.Frequency)}.{nameof(FrequencyDto.Type)}",
                $"{nameof(Habit.Frequency)}.{nameof(Frequency.Type)}"),
            new SortMapping(
                $"{nameof(HabitDto.Frequency)}.{nameof(FrequencyDto.TimesInPeriod)}",
                $"{nameof(Habit.Frequency)}.{nameof(Frequency.TimesInPeriod)}"),
            new SortMapping(
                $"{nameof(HabitDto.Target)}.{nameof(TargetDto.Value)}",
                $"{nameof(Habit.Target)}.{nameof(Target.Value)}"),
            new SortMapping(
                $"{nameof(HabitDto.Target)}.{nameof(TargetDto.Unit)}",
                $"{nameof(Habit.Target)}.{nameof(Target.Unit)}"),
            new SortMapping(nameof(HabitDto.Status), nameof(Habit.Status)),
            new SortMapping(nameof(HabitDto.EndDate), nameof(Habit.EndDate)),
            new SortMapping(nameof(HabitDto.CreatedAtUtc), nameof(Habit.CreatedAtUtc)),
            new SortMapping(nameof(HabitDto.UpdatedAtUtc), nameof(Habit.UpdatedAtUtc)),
            new SortMapping(nameof(HabitDto.LastCompletedAtUtc), nameof(Habit.LastCompletedAtUtc))
        ]
    };

    public static Habit ToEntity(this CreateHabitDto habitDto)
    {
        Habit habit = new()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = habitDto.Name,
            Description = habitDto.Description,
            Type = habitDto.Type,
            Frequency = new Frequency()
            {
                Type = habitDto.Frequency.Type,
                TimesInPeriod = habitDto.Frequency.TimesInPeriod,
            },
            Target = new Target()
            {
                Unit = habitDto.Target.Unit,
                Value = habitDto.Target.Value,
            },
            Status = HabitStatus.Ongoing,
            IsArchived = false,
            EndDate = habitDto.EndDate,
            Milestone = habitDto.Milestone is not null
                ? new Milestone()
                {
                    Target = habitDto.Milestone.Target,
                    Current = 0
                }
                : null,
            CreatedAtUtc = DateTime.UtcNow,
        };
        
        return habit;
    }

    public static HabitDto ToDto(this Habit habit)
    {
        return new HabitDto
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency = new FrequencyDto
            {
                Type = habit.Frequency.Type,
                TimesInPeriod = habit.Frequency.TimesInPeriod
            },
            Target = new TargetDto
            {
                Value = habit.Target.Value,
                Unit = habit.Target.Unit
            },
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone == null
                ? null
                : new MilestoneDto
                {
                    Target = habit.Milestone.Target,
                    Current = habit.Milestone.Current
                },
            CreatedAtUtc = habit.CreatedAtUtc,
            UpdatedAtUtc = habit.UpdatedAtUtc,
            LastCompletedAtUtc = habit.LastCompletedAtUtc,
        };
    }

    public static void UpdateFromDto(this Habit habit, UpdateHabitDto habitDto)
    {
        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.Type = habitDto.Type;
        habit.EndDate = habitDto.EndDate;
        habit.Frequency = new Frequency
        {
            Type = habitDto.Frequency.Type,
            TimesInPeriod = habitDto.Frequency.TimesInPeriod
        };
        habit.Target = new Target
        {
            Value = habitDto.Target.Value,
            Unit = habitDto.Target.Unit
        };
        if (habitDto.Milestone is not null)
        {
            habit.Milestone ??= new Milestone();
            habit.Milestone.Target = habitDto.Milestone.Target;

        }
        
        habit.UpdatedAtUtc = DateTime.UtcNow;
    }
}
