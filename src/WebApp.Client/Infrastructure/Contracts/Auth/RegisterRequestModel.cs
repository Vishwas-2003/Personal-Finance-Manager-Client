namespace WebApp.Client.Infrastructure.Contracts.Auth;

public sealed class RegisterRequestModel
{
    public string Name { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

