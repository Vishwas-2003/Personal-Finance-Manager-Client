namespace WebApp.Client.Infrastructure.Contracts.Expenses;

public sealed class CategoryResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CategoryType { get; init; } = string.Empty;
}

