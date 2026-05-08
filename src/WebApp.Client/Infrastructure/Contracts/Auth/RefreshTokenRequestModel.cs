namespace WebApp.Client.Infrastructure.Contracts.Auth;

public sealed class RefreshTokenRequestModel
{
    public string RefreshToken { get; init; } = string.Empty;
}

