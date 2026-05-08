using System.Text;
using System.Text.Json;
using WebApp.Client.Constants;

namespace WebApp.Client.Infrastructure.Http;

public static class JsonHttp
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static StringContent CreateBody<T>(T value) =>
        new(JsonSerializer.Serialize(value, JsonOptions), Encoding.UTF8, AppConstants.Http.JsonMediaType);

    public static async Task<T> ReadOrThrowAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
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
        throw new ApiException(response.StatusCode, string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase : body);
    }

    public static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ApiException(response.StatusCode, string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase : body);
    }
}

