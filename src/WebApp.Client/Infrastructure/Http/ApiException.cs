using System.Net;

namespace WebApp.Client.Infrastructure.Http;

public sealed class ApiException(HttpStatusCode statusCode, string? message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}

