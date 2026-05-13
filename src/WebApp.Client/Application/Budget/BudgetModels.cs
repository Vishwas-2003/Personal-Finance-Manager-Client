namespace WebApp.Client.Application.Budget;

public sealed record AddBudgetInput(
    int CategoryId,
    decimal LimitAmount,
    decimal SpentAmount);

public sealed record BudgetItem(
    int Id,
    decimal LimitAmount,
    decimal SpentAmount,
    DateTime UpdatedAtUtc,
    int CategoryId,
    string CategoryName,
    string CategoryType);
