namespace WebApp.Client.Application.Budget;

public sealed record AddBudgetInput(
    int CategoryId,
    decimal LimitAmount,
    decimal SpentAmount);

public sealed record UpdateBudgetInput(
    int Id,
    int CategoryId,
    decimal LimitAmount,
    decimal SpentAmount);

public sealed record BudgetListFilter(
    int? CategoryId,
    string? Keywords);

public sealed record BudgetItem(
    int Id,
    decimal LimitAmount,
    decimal SpentAmount,
    DateTime UpdatedAtUtc,
    int CategoryId,
    string CategoryName,
    string CategoryType,
    bool InActive);
