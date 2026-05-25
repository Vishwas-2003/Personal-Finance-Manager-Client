namespace WebApp.Client.Infrastructure.Contracts.Income;

public sealed class IncomeResponseModel
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public string Source { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public DateTime Date { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public bool InActive { get; init; }
    public IncomeCategoryResponseModel Category { get; init; } = new();
}

