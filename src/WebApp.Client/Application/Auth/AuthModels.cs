namespace WebApp.Client.Application.Auth;

public sealed record AuthResult(
    string Email,
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAtUtc);

public sealed record RegisterInput(
    string Name,
    string MobileNumber,
    int Age,
    string Address,
    string Email,
    string Password);

