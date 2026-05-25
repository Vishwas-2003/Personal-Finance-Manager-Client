using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;

namespace WebApp.Client.ConsoleUi;

public sealed class ConsoleClearCoordinator : IConsoleClearCoordinator
{
    private readonly IConsole _console = new SystemConsole();

    public async Task ClearAfterLoginAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(TimeSpan.FromSeconds(AppConstants.Values.ClearDelaySeconds), cancellationToken);
        ClearScreen();
    }

    public void ClearScreen()
    {
        _console.Clear();
        _console.WriteLine(AppConstants.Titles.AppName, ConsoleMessageKind.Title);
        _console.WriteLine(AppConstants.Titles.AppDivider, ConsoleMessageKind.Muted);
    }
}
