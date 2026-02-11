using System.Linq;
using System.Security.Claims;

namespace Acheve.Authentication.Events;

public static class ClaimsPrincipalExtensions
{
    public static string ToFormattedLog(this ClaimsPrincipal? principal)
    {
        if (principal is null)
        {
            return "No principal created";
        }

        // Try to identify the name of the principal
        var name = principal.Identity?.Name
                   ?? principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                   ?? principal.FindFirst(c => c.Type == "sub")?.Value;
        var identities = string.Join(", ", principal.Identities.Select(i => $"{i.AuthenticationType} - Authenticated: {i.IsAuthenticated}"));

        return $"{name} with {principal.Identities.Count()} identity(es): {identities})";
    }
}
