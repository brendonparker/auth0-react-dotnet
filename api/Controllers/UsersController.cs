using Auth0.Core.Exceptions;
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
    private readonly Auth0ManagementApiTokenProvider _apiTokenProvider;

    public UsersController(
        ILogger<UsersController> logger,
        ManagementApiClient auth0Management)
    {
        _logger = logger;
        _auth0Management = auth0Management;
    }

    [HttpGet]
    public async Task<object[]> Get()
    {
        var paginationInfo = new PaginationInfo(0, 50, false);
        var members = await _auth0Management.Organizations.GetAllMembersAsync(User.Organizationid(), paginationInfo);
        return members.Select(x => new UserDTO
        {
            UserId = x.UserId,
            Email = x.Email,
            Name = x.Name,
        }).ToArray();
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
        await _auth0Management.Organizations.AddMembersAsync(User.Organizationid(), new OrganizationAddMembersRequest
        {
            Members = new List<string> { newUser.UserId }
        });
    }
}

public class UserDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
