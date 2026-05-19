using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Income;

namespace WebApp.Client.Application.Summary;

public sealed record IncomeSummarySubCategorySection(
    int CategoryId,
    string CategoryName,
    decimal Subtotal,
    IReadOnlyList<IncomeItem> IncomeEntries);

public sealed record IncomeSummaryCategoryTypeSection(
    int CategoryTypeId,
    string CategoryTypeName,
    decimal SectionTotal,
    IReadOnlyList<IncomeSummarySubCategorySection> SubCategorySections);

public sealed record IncomeSummary(
    IReadOnlyList<IncomeSummaryCategoryTypeSection> CategoryTypeSections,
    decimal TotalIncome);

public sealed record ExpenseSummarySubCategorySection(
    int CategoryId,
    string CategoryName,
    decimal Subtotal,
    IReadOnlyList<ExpenseItem> ExpenseEntries);

public sealed record ExpenseSummaryCategoryTypeSection(
    int CategoryTypeId,
    string CategoryTypeName,
    decimal SectionTotal,
    IReadOnlyList<ExpenseSummarySubCategorySection> SubCategorySections);

public sealed record ExpenseSummary(
    IReadOnlyList<ExpenseSummaryCategoryTypeSection> CategoryTypeSections,
    decimal TotalExpense);

public sealed record BalanceSummaryFilter(DateTime? FromDate, DateTime? ToDate);

public sealed record BalanceSummaryCreditLine(
    int Id,
    decimal Amount,
    DateTime Date,
    string Source,
    string? Notes,
    string CategoryName,
    string CategoryType);

public sealed record BalanceSummaryDebitLine(
    int Id,
    decimal Amount,
    DateTime Date,
    string? Description,
    string CategoryName,
    string CategoryType);

public sealed record BalanceSummary(
    IReadOnlyList<BalanceSummaryCreditLine> Credits,
    IReadOnlyList<BalanceSummaryDebitLine> Debits,
    decimal TotalCredit,
    decimal TotalDebit,
    decimal Balance);
