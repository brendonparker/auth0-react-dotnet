using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace api;

public interface IAuth0CustomApi
{
    Task<Role[]> GetRolesForUser(string orgId, string userId);
    Task SetRolesForUser(string orgId, string userId, string[] roles);
}

public class Auth0CustomApi : IAuth0CustomApi
{
    private readonly ILogger<Auth0CustomApi> logger;
    private readonly Auth0ManagementApiTokenProvider tokenProvider;
    private readonly IHttpClientFactory httpClientFactory;

    public Auth0CustomApi(
        ILogger<Auth0CustomApi> logger,
        Auth0ManagementApiTokenProvider tokenProvider,
        IHttpClientFactory httpClientFactory)
    {
        this.logger = logger;
        this.tokenProvider = tokenProvider;
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<Role[]> GetRolesForUser(string orgId, string userId)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/organizations/{orgId}/members/{userId}/roles");
        return await SendAsync<Role[]>(message);
    }

    public async Task SetRolesForUser(string orgId, string userId, string[] roles)
    {
        var messageRoles = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/roles");
        var resRoles = await SendAsync<Role[]>(messageRoles);
        var auth0Roles = resRoles.Where(x => roles.Contains(x.Name));

        var payload = new
        {
            roles = auth0Roles.Select(x => x.Id).ToArray()
        };

        var message = new HttpRequestMessage(HttpMethod.Post, $"/api/v2/organizations/{orgId}/members/{userId}/roles");
        message.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        await SendAsync<object>(message);
    }

    private async Task<T> SendAsync<T>(HttpRequestMessage requestMessage)
    {
        var client = httpClientFactory.CreateClient(Constants.Auth0Client);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await tokenProvider.GetTokenAsync());
        var responseMessage = await client.SendAsync(requestMessage);
        var content = await responseMessage.Content.ReadAsStringAsync();
        logger.LogInformation("Response for {URL} {ResponseBody}", requestMessage.RequestUri, content);
        responseMessage.EnsureSuccessStatusCode();
        if (string.IsNullOrWhiteSpace(content)) return default(T);
        return JsonSerializer.Deserialize<T>(content);
    }
}

public class Role
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
}