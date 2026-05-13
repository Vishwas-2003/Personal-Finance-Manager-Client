namespace WebApp.Client.Infrastructure.Contracts.Budget;

public sealed class BudgetCategoryResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CategoryType { get; init; } = string.Empty;
}
