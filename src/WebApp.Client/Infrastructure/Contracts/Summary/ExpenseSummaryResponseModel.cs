namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class ExpenseSummaryResponseModel
{
    public List<ExpenseSummaryCategoryTypeGroupModel> CategoryTypes { get; init; } = [];
    public decimal TotalExpense { get; init; }
}
