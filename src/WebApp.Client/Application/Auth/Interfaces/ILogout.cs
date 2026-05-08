namespace WebApp.Client.Application.Auth.Interfaces;

public interface ILogout
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

