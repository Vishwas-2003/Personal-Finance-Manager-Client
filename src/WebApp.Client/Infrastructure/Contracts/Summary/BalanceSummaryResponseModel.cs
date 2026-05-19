namespace WebApp.Client.Infrastructure.Contracts.Summary;

public sealed class BalanceSummaryResponseModel
{
    public IReadOnlyList<BalanceSummaryCreditLineModel> Credits { get; init; } = [];
    public IReadOnlyList<BalanceSummaryDebitLineModel> Debits { get; init; } = [];
    public decimal TotalCredit { get; init; }
    public decimal TotalDebit { get; init; }
    public decimal Balance { get; init; }
}

public sealed class BalanceSummaryCreditLineModel
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string Source { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string CategoryType { get; init; } = string.Empty;
}

public sealed class BalanceSummaryDebitLineModel
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public string? Description { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string CategoryType { get; init; } = string.Empty;
}
