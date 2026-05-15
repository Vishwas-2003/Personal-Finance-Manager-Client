namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class IncomeSummaryCategoryTypeGroupModel
{
    public int CategoryTypeId { get; init; }
    public string CategoryType { get; init; } = string.Empty;
    public decimal Subtotal { get; init; }
    public List<IncomeSummaryCategoryGroupModel> Categories { get; init; } = [];
}
