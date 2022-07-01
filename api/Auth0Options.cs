namespace api;

public class Auth0Options
{
    public const string Key = "Auth0";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
}