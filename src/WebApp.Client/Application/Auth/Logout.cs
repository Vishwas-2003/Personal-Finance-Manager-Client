using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Auth;

public sealed class Logout(ISessionStore sessionStore, ISessionAccessor sessionAccessor) : ILogout
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        sessionAccessor.Set(null);
        await sessionStore.ClearAsync(cancellationToken);
    }
}

