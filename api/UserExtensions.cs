using System.Security.Claims;

namespace api;
public static class UserExtensions
{
    public static string? CustomerId(this ClaimsPrincipal source) =>
    source.Claims.FirstOrDefault(x => x.Type == "https://localhost/org")?.Value;
}
