namespace WebApp.Client.Application.Income;

public sealed record AddIncomeInput(
    decimal Amount,
    int CategoryId,
    DateTime Date,
    string Source,
    string? Notes);

public sealed record UpdateIncomeInput(
    int Id,
    decimal Amount,
    int CategoryId,
    DateTime Date,
    string Source,
    string? Notes);

public sealed record IncomeListFilter(
    int? CategoryId,
    DateTime? FromDate,
    DateTime? ToDate,
    string? Keywords);

public sealed record IncomeItem(
    int Id,
    decimal Amount,
    int CategoryId,
    string CategoryName,
    string CategoryType,
    DateTime Date,
    string Source,
    string? Notes,
    DateTime CreatedAtUtc,
    bool InActive);
