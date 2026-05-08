namespace WebApp.Client.Application.Auth.Interfaces;

public interface IRegisterUser
{
    Task ExecuteAsync(RegisterInput input, CancellationToken cancellationToken);
}

