using System.Linq;
using System.Security.Claims;

namespace Acheve.Authentication.Events
{
    public static class ClaimsPrincipalExtensions
    {
        public static string ToFormattedLog(this ClaimsPrincipal principal)
        {
            var identities = string.Join(", ", principal.Identities.Select(i => $"{i.AuthenticationType} - Authenticated: {i.IsAuthenticated}"));
            return $"{principal.Identity.Name} - Identities: {identities})";
        }
    }
}
