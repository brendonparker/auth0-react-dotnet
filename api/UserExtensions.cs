using System.Security.Claims;

namespace api;
public static class UserExtensions
{
    public static string CustomerId(this ClaimsPrincipal source) =>
        source.Claims.First(x => x.Type == Constants.OrgClaim).Value;
    public static string OrganizationId(this ClaimsPrincipal source) =>
        source.Claims.First(x => x.Type == Constants.OrgIdClaim).Value;
}
