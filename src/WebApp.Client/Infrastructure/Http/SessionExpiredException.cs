namespace WebApp.Client.Infrastructure.Http;

public sealed class SessionExpiredException(string? message = null)
    : Exception(string.IsNullOrWhiteSpace(message) ? "Session expired." : message);
