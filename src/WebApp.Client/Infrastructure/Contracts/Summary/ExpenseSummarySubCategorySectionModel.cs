using WebApp.Client.Infrastructure.Contracts.Expenses;

namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class ExpenseSummarySubCategorySectionModel
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal Subtotal { get; init; }
    public List<ExpenseResponseModel> ExpenseEntries { get; init; } = [];
}
