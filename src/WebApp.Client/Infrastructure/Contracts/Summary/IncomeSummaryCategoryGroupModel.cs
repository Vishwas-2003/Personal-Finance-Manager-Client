using WebApp.Client.Infrastructure.Contracts.Income;

namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class IncomeSummaryCategoryGroupModel
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal Subtotal { get; init; }
    public List<IncomeResponseModel> Items { get; init; } = [];
}
