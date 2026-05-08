using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session;
using WebApp.Client.Infrastructure.Session.Interfaces;
using WebApp.Client.Infrastructure.Token.Interfaces;

namespace WebApp.Client.Application.Auth;

public sealed class RegisterUser(IAuthApi authApi, IJwtUserIdReader userIdReader, ISessionStore sessionStore, ISessionAccessor sessionAccessor) : IRegisterUser
{
    public async Task ExecuteAsync(RegisterInput input, CancellationToken cancellationToken)
    {
        var auth = await authApi.RegisterAsync(input, cancellationToken);
        var session = CreateSessionOrThrow(auth);
        sessionAccessor.Set(session);
        await sessionStore.SaveAsync(session, cancellationToken);
    }

    private SessionModel CreateSessionOrThrow(AuthResult auth)
    {
        if (!userIdReader.TryReadUserId(auth.AccessToken, out var userId))
        {
            throw new InvalidOperationException(AppConstants.Messages.MissingUserIdInToken);
        }

        return new SessionModel(
            userId,
            auth.Email,
            auth.AccessToken,
            auth.RefreshToken,
            auth.AccessTokenExpiresAtUtc);
    }
}

