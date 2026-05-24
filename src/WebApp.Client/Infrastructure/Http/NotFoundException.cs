namespace WebApp.Client.Infrastructure.Http;

public sealed class NotFoundException(string? message = null)
    : Exception(string.IsNullOrWhiteSpace(message) ? "The requested resource was not found." : message);
