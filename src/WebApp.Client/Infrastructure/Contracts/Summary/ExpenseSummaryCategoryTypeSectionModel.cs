namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class ExpenseSummaryCategoryTypeSectionModel
{
    public int CategoryTypeId { get; init; }
    public string CategoryTypeName { get; init; } = string.Empty;
    public decimal SectionTotal { get; init; }
    public List<ExpenseSummarySubCategorySectionModel> SubCategorySections { get; init; } = [];
}
