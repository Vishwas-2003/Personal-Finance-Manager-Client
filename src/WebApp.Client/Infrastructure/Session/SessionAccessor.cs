using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Infrastructure.Session;

public sealed class SessionAccessor : ISessionAccessor
{
    public SessionModel? Current { get; private set; }

    public void Set(SessionModel? session) => Current = session;
}

