using DevHabit.Api.DTOs.Common;

namespace DevHabit.Api.Services;

public class LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
{
    public LinkDto CreateLink(
        string endpointName,
        string rel,
        string method,
        object? values = null,
        string? controller = null)
    {
        string? href = linkGenerator.GetUriByAction(
            httpContextAccessor.HttpContext!,
            endpointName,
            controller,
            values);

        if (href is null)
        {
            throw new InvalidOperationException($"The link for '{endpointName}' is not defined");
        }
        return new LinkDto
        {
            Href = href,
            Rel = rel,
            Method = method
        };
    }
    
}
