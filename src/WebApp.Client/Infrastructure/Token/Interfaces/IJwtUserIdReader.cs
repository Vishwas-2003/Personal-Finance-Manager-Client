namespace WebApp.Client.Infrastructure.Token.Interfaces;

public interface IJwtUserIdReader
{
    bool TryReadUserId(string jwt, out int userId);
}

