namespace WebApp.Client.Application.Auth.Interfaces;

public interface IAuthApi
{
    Task<AuthResult> RegisterAsync(RegisterInput input, CancellationToken cancellationToken);
    Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken);
    Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
}

