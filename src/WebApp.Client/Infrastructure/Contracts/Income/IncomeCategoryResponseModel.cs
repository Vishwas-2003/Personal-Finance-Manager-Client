namespace WebApp.Client.Infrastructure.Contracts.Income;

public sealed class IncomeCategoryResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CategoryType { get; init; } = string.Empty;
}

