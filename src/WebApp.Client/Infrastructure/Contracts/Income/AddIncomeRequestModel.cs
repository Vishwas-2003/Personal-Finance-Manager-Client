namespace WebApp.Client.Infrastructure.Contracts.Income;

public sealed class AddIncomeRequestModel
{
    public int UserId { get; init; }
    public decimal Amount { get; init; }
    public int CategoryId { get; init; }
    public DateTime Date { get; init; }
    public string Source { get; init; } = string.Empty;
    public string? Notes { get; init; }
}

