namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IListExpenses
{
    Task<IReadOnlyList<ExpenseItem>> ExecuteAsync(ExpenseListFilter? filter, CancellationToken cancellationToken);
}

