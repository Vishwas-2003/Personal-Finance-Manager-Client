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

    public static SessionExpiredException? FindSessionExpired(Exception exception)
    {
        if (exception is SessionExpiredException sessionExpired)
        {
            return sessionExpired;
        }

        if (exception is AggregateException aggregate)
        {
            foreach (var inner in aggregate.InnerExceptions)
            {
                var found = FindSessionExpired(inner);
                if (found is not null)
                {
                    return found;
                }
            }
        }

        return exception.InnerException is not null
            ? FindSessionExpired(exception.InnerException)
            : null;
    }
}
