namespace WebApp.Client.Infrastructure.Contracts.Expenses;

public sealed class ExpenseResponseModel
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public CategoryResponseModel Category { get; init; } = new();
    public string? Description { get; init; }
    public DateTime Date { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}

