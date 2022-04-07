using System.Security.Claims;

namespace Recarro.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal principal)
            => principal.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool isAdmin(this ClaimsPrincipal principal)
            => principal.IsInRole(WebConstants.AdministratorRoleName);
    }
}
