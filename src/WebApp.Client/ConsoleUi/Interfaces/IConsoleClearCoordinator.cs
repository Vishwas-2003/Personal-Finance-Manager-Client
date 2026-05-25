namespace WebApp.Client.ConsoleUi.Interfaces;

public interface IConsoleClearCoordinator
{
    Task ClearAfterLoginAsync(CancellationToken cancellationToken = default);
    void ClearScreen();
}
