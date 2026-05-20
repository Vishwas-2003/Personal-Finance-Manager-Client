using System.Net;
using System.Text;
using System.Text.Json;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Contracts.Api;

namespace WebApp.Client.Infrastructure.Http;

public static class JsonHttp
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static StringContent CreateBody<T>(T value) =>
        new(JsonSerializer.Serialize(value, JsonOptions), Encoding.UTF8, AppConstants.Http.JsonMediaType);

    public static HttpResponseMessage CreateSessionExpiredResponse()
    {
        var body = JsonSerializer.Serialize(new ApiErrorResponseModel
        {
            Code = AppConstants.ErrorCodes.SessionExpired,
            Message = AppConstants.Messages.SessionExpired
        }, JsonOptions);

        return new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent(body, Encoding.UTF8, AppConstants.Http.JsonMediaType)
        };
    }

    public static async Task<T> ReadOrThrowAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            if (response.IsSuccessStatusCode)
            {
                await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var payload = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken);
                if (payload is null)
                {
                    throw new ApiException(response.StatusCode, AppConstants.Messages.EmptyResponseBody);
                }

                return payload;
            }

            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw CreateException(response.StatusCode, body, response.ReasonPhrase);
        }
        catch
        {
            throw;
        }
    }

    public static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw CreateException(response.StatusCode, body, response.ReasonPhrase);
        }
        catch
        {
            throw;
        }
    }

    private static Exception CreateException(HttpStatusCode statusCode, string? body, string? reasonPhrase)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            return CreateSessionExpiredException(body);
        }

        return new ApiException(statusCode, string.IsNullOrWhiteSpace(body) ? reasonPhrase : body);
    }

    public static SessionExpiredException CreateSessionExpiredException(string? body)
    {
        var error = TryParseApiError(body);
        if (error?.Code == AppConstants.ErrorCodes.SessionExpired && !string.IsNullOrWhiteSpace(error.Message))
        {
            return new SessionExpiredException(error.Message);
        }

        return new SessionExpiredException(AppConstants.Messages.SessionExpired);
    }

    private static ApiErrorResponseModel? TryParseApiError(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ApiErrorResponseModel>(body, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
