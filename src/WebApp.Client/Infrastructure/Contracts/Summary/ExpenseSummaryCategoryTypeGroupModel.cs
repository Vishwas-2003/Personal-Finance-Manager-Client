namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class ExpenseSummaryCategoryTypeGroupModel
{
    public int CategoryTypeId { get; init; }
    public string CategoryType { get; init; } = string.Empty;
    public decimal Subtotal { get; init; }
    public List<ExpenseSummaryCategoryGroupModel> Categories { get; init; } = [];
}
