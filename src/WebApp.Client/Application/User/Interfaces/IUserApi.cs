using WebApp.Client.Application.User;

namespace WebApp.Client.Application.User.Interfaces;

public interface IUserApi
{
    Task<UserProfile> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
}
