namespace WebApp.Client.Infrastructure.Http;

public sealed class MustLoginException(string? message = null)
    : Exception(string.IsNullOrWhiteSpace(message) ? "You must login first." : message);
