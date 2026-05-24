namespace WebApp.Client.Infrastructure.Http;

public sealed class ConflictException(string? message = null)
    : Exception(string.IsNullOrWhiteSpace(message) ? "The request could not be completed due to a conflict." : message);
