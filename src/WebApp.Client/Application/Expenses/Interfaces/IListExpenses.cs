namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IListExpenses
{
    Task<IReadOnlyList<ExpenseItem>> ExecuteAsync(CancellationToken cancellationToken);
}

