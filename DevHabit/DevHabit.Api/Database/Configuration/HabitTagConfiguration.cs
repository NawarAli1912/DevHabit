using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configuration;

public class HabitTagConfiguration : IEntityTypeConfiguration<HabitTag>
{
    public void Configure(EntityTypeBuilder<HabitTag> builder)
    {
        builder.HasKey(ht => new { ht.HabitId, ht.TagId });

        builder.Property(ht => ht.HabitId).HasMaxLength(512);
        builder.Property(ht => ht.TagId).HasMaxLength(500);

        builder.HasOne<Habit>()
            .WithMany(h => h.HabitTags)
            .HasForeignKey(ht => ht.HabitId);

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(ht => ht.TagId);
    }
}
