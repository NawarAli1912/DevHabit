using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configuration;

public class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasMaxLength(512);
        builder.Property(x => x.Name).HasMaxLength(512);
        builder.Property(x => x.Description).HasMaxLength(512);

        builder.OwnsOne(x => x.Frequency);
        builder.OwnsOne(x => x.Target);
        builder.OwnsOne(x => x.Milestone);
    }
}
