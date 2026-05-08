namespace WebApp.Client.Infrastructure.Contracts.Expenses;

public sealed class AddExpenseRequestModel
{
    public int UserId { get; init; }
    public decimal Amount { get; init; }
    public int CategoryId { get; init; }
    public string? Description { get; init; }
    public DateTime Date { get; init; }
}

