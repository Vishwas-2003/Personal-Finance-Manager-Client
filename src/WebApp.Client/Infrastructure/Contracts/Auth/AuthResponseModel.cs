namespace WebApp.Client.Infrastructure.Contracts.Auth;

public sealed class AuthResponseModel
{
    public string Email { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime AccessTokenExpiresAtUtc { get; init; }
}

