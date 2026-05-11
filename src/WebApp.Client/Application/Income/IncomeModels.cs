namespace WebApp.Client.Application.Income;

public sealed record AddIncomeInput(
    decimal Amount,
    int CategoryId,
    DateTime Date,
    string Source,
    string? Notes);

public sealed record IncomeItem(
    int Id,
    decimal Amount,
    int CategoryId,
    string CategoryName,
    string CategoryType,
    DateTime Date,
    string Source,
    string? Notes,
    DateTime CreatedAtUtc);

