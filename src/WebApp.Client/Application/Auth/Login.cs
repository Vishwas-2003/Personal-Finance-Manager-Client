using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session;
using WebApp.Client.Infrastructure.Session.Interfaces;
using WebApp.Client.Infrastructure.Token.Interfaces;

namespace WebApp.Client.Application.Auth;

public sealed class Login(IAuthApi authApi, IJwtUserIdReader userIdReader, ISessionStore sessionStore, ISessionAccessor sessionAccessor) : ILogin
{
    public async Task ExecuteAsync(string email, string password, CancellationToken cancellationToken)
    {
        var auth = await authApi.LoginAsync(email, password, cancellationToken);
        if (!userIdReader.TryReadUserId(auth.AccessToken, out var userId))
        {
            throw new InvalidOperationException(AppConstants.Messages.MissingUserIdInToken);
        }

        var session = new SessionModel(userId, auth.Email, auth.AccessToken, auth.RefreshToken, auth.AccessTokenExpiresAtUtc);
        sessionAccessor.Set(session);
        await sessionStore.SaveAsync(session, cancellationToken);
    }
}

