using WebApp.Client.Application.Auth;
using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Auth;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Auth;

public sealed class AuthApi(IHttpClientFactory httpClientFactory) : IAuthApi
{
    public async Task<AuthResult> RegisterAsync(RegisterInput input, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Unauthenticated);
        var request = new RegisterRequestModel
        {
            Name = input.Name,
            MobileNumber = input.MobileNumber,
            Age = input.Age,
            Address = input.Address,
            Email = input.Email,
            Password = input.Password
        };
        var response = await client.PostAsync(
            RouteConstants.Auth.Register,
            JsonHttp.CreateBody(request),
            cancellationToken);

        var payload = await JsonHttp.ReadOrThrowAsync<AuthResponseModel>(response, cancellationToken);
        return new AuthResult(payload.Email, payload.AccessToken, payload.RefreshToken, payload.AccessTokenExpiresAtUtc);
    }

    public async Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Unauthenticated);
        var request = new LoginRequestModel { Email = email, Password = password };
        var response = await client.PostAsync(
            RouteConstants.Auth.Login,
            JsonHttp.CreateBody(request),
            cancellationToken);

        var payload = await JsonHttp.ReadOrThrowAsync<AuthResponseModel>(response, cancellationToken);
        return new AuthResult(payload.Email, payload.AccessToken, payload.RefreshToken, payload.AccessTokenExpiresAtUtc);
    }

    public async Task<AuthResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Unauthenticated);
        var request = new RefreshTokenRequestModel { RefreshToken = refreshToken };
        var response = await client.PostAsync(
            RouteConstants.Auth.Refresh,
            JsonHttp.CreateBody(request),
            cancellationToken);

        var payload = await JsonHttp.ReadOrThrowAsync<AuthResponseModel>(response, cancellationToken);
        return new AuthResult(payload.Email, payload.AccessToken, payload.RefreshToken, payload.AccessTokenExpiresAtUtc);
    }
}

