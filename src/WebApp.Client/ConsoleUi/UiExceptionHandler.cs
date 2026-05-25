using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;

namespace WebApp.Client.ConsoleUi;

public sealed class UiExceptionHandler(ILogout logout) : IUiExceptionHandler
{
    private readonly IConsole _console = new SystemConsole();

    public async Task<bool> TryHandleAsync(Exception exception, CancellationToken cancellationToken = default)
    {
        var sessionExpired = ExceptionHelper.FindSessionExpired(exception);
        if (sessionExpired is not null)
        {
            await logout.ExecuteAsync(cancellationToken);
            _console.WriteLine(sessionExpired.Message, ConsoleMessageKind.Warning);
            return true;
        }

        var mustLogin = ExceptionHelper.FindMustLogin(exception);
        if (mustLogin is not null)
        {
            _console.WriteLine(mustLogin.Message, ConsoleMessageKind.Warning);
            return true;
        }

        if (ExceptionHelper.IsConnectionError(exception))
        {
            _console.WriteLine(AppConstants.Messages.SomethingWentWrong, ConsoleMessageKind.Error);
            return true;
        }

        var notFound = ExceptionHelper.FindNotFound(exception);
        if (notFound is not null)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, notFound.Message), ConsoleMessageKind.Error);
            return true;
        }

        var conflict = ExceptionHelper.FindConflict(exception);
        if (conflict is not null)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, conflict.Message), ConsoleMessageKind.Error);
            return true;
        }

        var badRequest = ExceptionHelper.FindBadRequest(exception);
        if (badRequest is not null)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, badRequest.Message), ConsoleMessageKind.Error);
            return true;
        }

        var apiException = ExceptionHelper.FindApiException(exception);
        if (apiException is not null)
        {
            _console.WriteLine(
                string.Format(AppConstants.Messages.ApiErrorFormat, (int)apiException.StatusCode, apiException.Message),
                ConsoleMessageKind.Error);
            return true;
        }

        var argumentException = ExceptionHelper.FindArgumentException(exception);
        if (argumentException is not null)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, argumentException.Message), ConsoleMessageKind.Error);
            return true;
        }

        var invalidOperation = ExceptionHelper.FindInvalidOperation(exception);
        if (invalidOperation is not null)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, invalidOperation.Message), ConsoleMessageKind.Error);
            return true;
        }

        _console.WriteLine(AppConstants.Messages.SomethingWentWrong, ConsoleMessageKind.Error);
        return true;
    }
}
