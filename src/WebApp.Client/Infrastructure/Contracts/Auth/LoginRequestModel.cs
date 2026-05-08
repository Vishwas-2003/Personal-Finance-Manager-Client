namespace WebApp.Client.Infrastructure.Contracts.Auth;

public sealed class LoginRequestModel
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

