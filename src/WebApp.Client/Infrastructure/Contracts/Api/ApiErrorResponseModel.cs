namespace WebApp.Client.Infrastructure.Contracts.Api;

public sealed class ApiErrorResponseModel
{
    public string Code { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
