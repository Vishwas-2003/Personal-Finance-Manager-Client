namespace WebApp.Client.Infrastructure.Contracts.User;

public sealed class UserResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Address { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
