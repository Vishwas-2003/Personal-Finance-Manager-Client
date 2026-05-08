namespace WebApp.Client.ConsoleUi.Auth.Interfaces;

public interface IAuthUi
{
    Task<bool> RunAsync(int choice);
}

