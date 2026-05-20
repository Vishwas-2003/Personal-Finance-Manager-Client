using WebApp.Client.Application.User;
using WebApp.Client.Application.User.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.User;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.User;

public sealed class UserApi(IHttpClientFactory httpClientFactory) : IUserApi
{
    public async Task<UserProfile> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.User.GetByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<UserResponseModel>(response, cancellationToken);
        return new UserProfile(
            payload.Id,
            payload.Name,
            payload.MobileNumber,
            payload.Age,
            payload.Address,
            payload.Email);
    }
}
