namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class IncomeSummaryResponseModel
{
    public List<IncomeSummaryCategoryTypeGroupModel> CategoryTypes { get; init; } = [];
    public decimal TotalIncome { get; init; }
}
