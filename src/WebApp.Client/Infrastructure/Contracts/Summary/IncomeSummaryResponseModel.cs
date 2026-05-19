namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class IncomeSummaryResponseModel
{
    public List<IncomeSummaryCategoryTypeSectionModel> CategoryTypeSections { get; init; } = [];
    public decimal TotalIncome { get; init; }
}
