namespace WebApp.Client.Application.Auth.Interfaces;

public interface ILogin
{
    Task ExecuteAsync(string email, string password, CancellationToken cancellationToken);
}

