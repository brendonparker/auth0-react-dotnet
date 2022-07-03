using System.Text.Json;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ManagementApiClient _auth0Management;
    private readonly IAuth0CustomApi _auth0Api;
    private readonly Auth0ManagementApiTokenProvider _apiTokenProvider;

    public UsersController(
        ILogger<UsersController> logger,
        ManagementApiClient auth0Management,
        IAuth0CustomApi auth0Api)
    {
        _logger = logger;
        _auth0Management = auth0Management;
        _auth0Api = auth0Api;
    }

    [HttpGet]
    public async Task<UserDTO[]> Get()
    {
        var paginationInfo = new PaginationInfo(0, 50, false);
        var members = await _auth0Management.Organizations.GetAllMembersAsync(User.OrganizationId(), paginationInfo);

        var users = members.Select(x => new UserDTO
        {
            UserId = x.UserId,
            Email = x.Email,
            Name = x.Name,
        }).ToArray();

        foreach (var user in users)
        {
            var roles = await _auth0Api.GetRolesForUser(User.OrganizationId(), user.UserId);
            user.Roles = roles.Select(x => x.Name).ToArray();
        }

        return users;
    }

    [HttpPost]
    public async Task Post([FromBody] UserDTO user)
    {
        var existingUsers = await _auth0Management.Users.GetUsersByEmailAsync(user.Email);
        _logger.LogInformation("Found {UserCount} existing users for email: {Email}", existingUsers.Count, user.Email);
        var existingUser = existingUsers.FirstOrDefault();
        var newUser = existingUser ?? await _auth0Management.Users.CreateAsync(new UserCreateRequest
        {
            Connection = "Username-Password-Authentication",
            Email = user.Email,
            VerifyEmail = false,
            Password = "Default123!"
        });

        _logger.LogInformation("Adding User to Organization");
        await _auth0Management.Organizations.AddMembersAsync(User.OrganizationId(), new OrganizationAddMembersRequest
        {
            Members = new List<string> { newUser.UserId }
        });
    }

    [HttpPut("{userid}/role")]
    public async Task SetRole([FromRoute] string userid, [FromBody] UpdateRoleDTO updateRole)
    {
        await _auth0Api.SetRolesForUser(User.OrganizationId(), userid, new[] { updateRole.Role });
    }

    [HttpDelete("{userid}")]
    public async Task Delete([FromRoute] string userId)
    {
        _logger.LogInformation("Adding User to Organization");
        await _auth0Management.Organizations.DeleteMemberAsync(User.OrganizationId(), new OrganizationDeleteMembersRequest
        {
            Members = new List<string> { userId }
        });
    }
}

public class UpdateRoleDTO
{
    public string Role { get; set; } = "User";
}

public class UserDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string[] Roles { get; set; } = new string[0];
}
