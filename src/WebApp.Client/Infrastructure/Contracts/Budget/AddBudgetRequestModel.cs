namespace WebApp.Client.Infrastructure.Contracts.Budget;

public sealed class AddBudgetRequestModel
{
    public int UserId { get; init; }
    public int CategoryId { get; init; }
    public decimal LimitAmount { get; init; }
    public decimal SpentAmount { get; init; }
}
