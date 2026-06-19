using System.Diagnostics.CodeAnalysis;

namespace DevHabit.Api.Services.Sorting;

[SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed",
    Justification = "TSource/TDestination identify the mapping so the provider can resolve it from DI.")]
public sealed class SortMappingDefinition<TSource, TDestination> : ISortMappingDefinition
{
    public required SortMapping[] Mappings { get; init; }
}
