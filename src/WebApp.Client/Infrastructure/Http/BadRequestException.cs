namespace WebApp.Client.Infrastructure.Http;

public sealed class BadRequestException(string? message = null)
    : Exception(string.IsNullOrWhiteSpace(message) ? "The request was invalid." : message);
