namespace WebApp.Client.Infrastructure.Session.Interfaces;

public interface ISessionStore
{
    Task<SessionModel?> LoadAsync(CancellationToken cancellationToken);
    Task SaveAsync(SessionModel session, CancellationToken cancellationToken);
    Task ClearAsync(CancellationToken cancellationToken);
}

