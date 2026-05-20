using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Infrastructure.Http;

public sealed class BearerTokenHandler(
    ISessionAccessor sessionAccessor,
    ISessionStore sessionStore,
    IServiceProvider serviceProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? await sessionStore.LoadAsync(cancellationToken);
        if (session is not null && !string.IsNullOrWhiteSpace(session.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.Http.BearerScheme, session.AccessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized || session is null)
        {
            return response;
        }

        response.Dispose();

        var refreshed = await TryRefreshAsync(session, cancellationToken);
        if (refreshed is null)
        {
            await ClearSessionAsync(cancellationToken);
            return JsonHttp.CreateSessionExpiredResponse();
        }

        var retry = await CloneAsync(request, cancellationToken);
        retry.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.Http.BearerScheme, refreshed.AccessToken);
        var retryResponse = await base.SendAsync(retry, cancellationToken);

        if (retryResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ClearSessionAsync(cancellationToken);
        }

        return retryResponse;
    }

    private async Task<SessionModel?> TryRefreshAsync(SessionModel session, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var authApi = scope.ServiceProvider.GetRequiredService<IAuthApi>();

        try
        {
            var auth = await authApi.RefreshAsync(session.RefreshToken, cancellationToken);
            var refreshed = session with
            {
                AccessToken = auth.AccessToken,
                RefreshToken = auth.RefreshToken,
                AccessTokenExpiresAtUtc = auth.AccessTokenExpiresAtUtc
            };

            sessionAccessor.Set(refreshed);
            await sessionStore.SaveAsync(refreshed, cancellationToken);
            return refreshed;
        }
        catch
        {
            return null;
        }
    }

    private async Task ClearSessionAsync(CancellationToken cancellationToken)
    {
        sessionAccessor.Set(null);
        await sessionStore.ClearAsync(cancellationToken);
    }

    private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content is null)
        {
            return clone;
        }

        var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
        clone.Content = new ByteArrayContent(bytes);
        foreach (var header in request.Content.Headers)
        {
            clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}
