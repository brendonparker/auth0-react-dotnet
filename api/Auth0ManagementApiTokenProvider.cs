using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace api;

public class Auth0ManagementApiTokenProvider
{
    private readonly ILogger<Auth0ManagementApiTokenProvider> _log;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Auth0Options _auth0Options;

    private string _Token;
    private DateTime _LastToken = DateTime.UtcNow.AddDays(-7);

    public Auth0ManagementApiTokenProvider(
        ILogger<Auth0ManagementApiTokenProvider> log,
        IHttpClientFactory httpClientFactory,
        Auth0Options auth0Options)
    {
        _log = log;
        _httpClientFactory = httpClientFactory;
        _auth0Options = auth0Options;
    }

    public async Task<string> GetTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_Token) || _LastToken.AddMinutes(5) <= DateTime.UtcNow)
        {
            var client = _httpClientFactory.CreateClient(Constants.Auth0Client);
            var message = new HttpRequestMessage(HttpMethod.Post, $"/oauth/token");
            message.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _auth0Options.ClientId,
                ["client_secret"] = _auth0Options.ClientSecret,
                ["audience"] = $"https://{_auth0Options.Domain}/api/v2/",
            });

            var res = await client.SendAsync(message);

            var tokenRes = await res.Content.ReadFromJsonAsync<TokenResponse>();
            _Token = tokenRes.AccessToken;
            _LastToken = DateTime.UtcNow;
            _log.LogInformation("Token: {Token}", _Token);
        }
        
        return _Token;
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}