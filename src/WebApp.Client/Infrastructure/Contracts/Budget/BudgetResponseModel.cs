namespace WebApp.Client.Infrastructure.Contracts.Budget;

public sealed class BudgetResponseModel
{
    public int Id { get; init; }
    public decimal LimitAmount { get; init; }
    public decimal SpentAmount { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public BudgetCategoryResponseModel Category { get; init; } = new();
}
