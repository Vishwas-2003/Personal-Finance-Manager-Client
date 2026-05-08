namespace WebApp.Client.Infrastructure.Session.Interfaces;

public interface ISessionAccessor
{
    SessionModel? Current { get; }
    void Set(SessionModel? session);
}

