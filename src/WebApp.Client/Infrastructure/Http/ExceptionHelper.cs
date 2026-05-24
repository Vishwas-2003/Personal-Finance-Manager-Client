using System.Net.Http;
using System.Net.Sockets;

namespace WebApp.Client.Infrastructure.Http;

public static class ExceptionHelper
{
    public static bool IsConnectionError(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is HttpRequestException or SocketException)
            {
                return true;
            }
        }

        return false;
    }

    public static SessionExpiredException? FindSessionExpired(Exception exception) =>
        FindException<SessionExpiredException>(exception);

    public static MustLoginException? FindMustLogin(Exception exception) =>
        FindException<MustLoginException>(exception);

    public static NotFoundException? FindNotFound(Exception exception) =>
        FindException<NotFoundException>(exception);

    public static ConflictException? FindConflict(Exception exception) =>
        FindException<ConflictException>(exception);

    public static BadRequestException? FindBadRequest(Exception exception) =>
        FindException<BadRequestException>(exception);

    public static ApiException? FindApiException(Exception exception) =>
        FindException<ApiException>(exception);

    public static ArgumentException? FindArgumentException(Exception exception) =>
        FindException<ArgumentException>(exception);

    public static InvalidOperationException? FindInvalidOperation(Exception exception) =>
        FindException<InvalidOperationException>(exception);

    private static TException? FindException<TException>(Exception exception)
        where TException : Exception
    {
        if (exception is TException match)
        {
            return match;
        }

        if (exception is AggregateException aggregate)
        {
            foreach (var inner in aggregate.InnerExceptions)
            {
                var found = FindException<TException>(inner);
                if (found is not null)
                {
                    return found;
                }
            }
        }

        return exception.InnerException is not null
            ? FindException<TException>(exception.InnerException)
            : null;
    }
}
