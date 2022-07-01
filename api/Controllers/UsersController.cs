using Auth0.ManagementApi;
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
        return members.Select(x => new
        {
            x.Email,
            x.Name,
            x.UserId
        }).ToArray();
    }
}
