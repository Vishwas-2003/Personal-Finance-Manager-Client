using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Income;

namespace WebApp.Client.Application.Summary;

public sealed record IncomeSummaryCategoryGroup(
    int CategoryId,
    string CategoryName,
    decimal Subtotal,
    IReadOnlyList<IncomeItem> Items);

public sealed record IncomeSummaryCategoryTypeGroup(
    int CategoryTypeId,
    string CategoryType,
    decimal Subtotal,
    IReadOnlyList<IncomeSummaryCategoryGroup> Categories);

public sealed record IncomeSummary(
    IReadOnlyList<IncomeSummaryCategoryTypeGroup> CategoryTypes,
    decimal TotalIncome);

public sealed record ExpenseSummaryCategoryGroup(
    int CategoryId,
    string CategoryName,
    decimal Subtotal,
    IReadOnlyList<ExpenseItem> Items);

public sealed record ExpenseSummaryCategoryTypeGroup(
    int CategoryTypeId,
    string CategoryType,
    decimal Subtotal,
    IReadOnlyList<ExpenseSummaryCategoryGroup> Categories);

public sealed record ExpenseSummary(
    IReadOnlyList<ExpenseSummaryCategoryTypeGroup> CategoryTypes,
    decimal TotalExpense);
