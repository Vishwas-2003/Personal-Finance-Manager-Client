using WebApp.Client.Application.User.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.User;

public sealed class GetUserProfile(IUserApi userApi, ISessionAccessor sessionAccessor) : IGetUserProfile
{
    public async Task<UserProfile> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        return await userApi.GetByUserIdAsync(session.UserId, cancellationToken);
    }
}
