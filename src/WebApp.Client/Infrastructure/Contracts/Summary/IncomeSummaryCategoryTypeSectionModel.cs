namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class IncomeSummaryCategoryTypeSectionModel
{
    public int CategoryTypeId { get; init; }
    public string CategoryTypeName { get; init; } = string.Empty;
    public decimal SectionTotal { get; init; }
    public List<IncomeSummarySubCategorySectionModel> SubCategorySections { get; init; } = [];
}
