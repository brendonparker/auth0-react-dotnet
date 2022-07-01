using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace api;

public class Auth0ManagementApiTokenProvider
{
    private static readonly HttpClient _client = new HttpClient();
    private readonly ILogger<Auth0ManagementApiTokenProvider> _log;
    private readonly Auth0Options _auth0Options;

    public Auth0ManagementApiTokenProvider(
        ILogger<Auth0ManagementApiTokenProvider> log,
        Auth0Options auth0Options)
    {
        _log = log;
        _auth0Options = auth0Options;
    }

    public async Task<string> GetTokenAsync()
    {
        var message = new HttpRequestMessage(HttpMethod.Post, $"https://{_auth0Options.Domain}/oauth/token");
        message.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _auth0Options.ClientId,
            ["client_secret"] =  _auth0Options.ClientSecret,
            ["audience"] = $"https://{_auth0Options.Domain}/api/v2/",
        });

        var res = await _client.SendAsync(message);

        var tokenRes = await res.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenRes.AccessToken;
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}