namespace WebApp.Client.Infrastructure.Session;

public sealed record SessionModel(
    int UserId,
    string Email,
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAtUtc);

