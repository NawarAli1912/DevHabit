using System.Linq.Expressions;
using DevHabit.Api.Entities;

namespace DevHabit.Api.DTOs.Tags;

public static class TagQueries
{
    public static Expression<Func<Tag, TagDto>> ProjectToDto()
    {
        return tag => new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAtUtc = tag.CreatedAtUtc,
            UpdatedAtUtc = tag.UpdatedAtUtc
        };
    }
}

public static class TagMappings
{
    public static Tag ToEntity(this CreateTagDto tagDto)
    {
        Tag tag = new()
        {
            Id = $"t_{Guid.CreateVersion7()}",
            Name = tagDto.Name,
            Description = tagDto.Description,
            CreatedAtUtc = DateTime.UtcNow
        };

        return tag;
    }

    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAtUtc = tag.CreatedAtUtc,
            UpdatedAtUtc = tag.UpdatedAtUtc
        };
    }

    public static void UpdateFromDto(this Tag tag, UpdateTagDto tagDto)
    {
        tag.Name = tagDto.Name;
        tag.Description = tagDto.Description;
        tag.UpdatedAtUtc = DateTime.UtcNow;
    }
}
