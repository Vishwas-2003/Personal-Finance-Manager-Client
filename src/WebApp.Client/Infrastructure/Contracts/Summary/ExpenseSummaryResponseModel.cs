namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class ExpenseSummaryResponseModel
{
    public List<ExpenseSummaryCategoryTypeSectionModel> CategoryTypeSections { get; init; } = [];
    public decimal TotalExpense { get; init; }
}
