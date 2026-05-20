namespace WebApp.Client.Application.User;

public sealed record UserProfile(
    int Id,
    string Name,
    string MobileNumber,
    int Age,
    string Address,
    string Email);
