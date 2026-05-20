using WebApp.Client.Application.User;

namespace WebApp.Client.Application.User.Interfaces;

public interface IGetUserProfile
{
    Task<UserProfile> ExecuteAsync(CancellationToken cancellationToken);
}
