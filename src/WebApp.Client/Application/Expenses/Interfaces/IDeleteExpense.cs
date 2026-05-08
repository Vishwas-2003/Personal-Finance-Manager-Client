namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IDeleteExpense
{
    Task ExecuteAsync(int expenseId, CancellationToken cancellationToken);
}

