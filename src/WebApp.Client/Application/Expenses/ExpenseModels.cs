namespace WebApp.Client.Application.Expenses;

public sealed record AddExpenseInput(
    decimal Amount,
    int CategoryId,
    string? Description,
    DateTime Date);

public sealed record ExpenseItem(
    int Id,
    decimal Amount,
    int CategoryId,
    string CategoryName,
    string CategoryType,
    string? Description,
    DateTime Date,
    DateTime CreatedAtUtc);

