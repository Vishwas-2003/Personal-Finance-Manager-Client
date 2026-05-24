namespace WebApp.Client.ConsoleUi.Interfaces;

public interface IUiExceptionHandler
{
    Task<bool> TryHandleAsync(Exception exception, CancellationToken cancellationToken = default);
}
