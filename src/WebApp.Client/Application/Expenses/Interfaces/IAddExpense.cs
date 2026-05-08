namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IAddExpense
{
    Task ExecuteAsync(AddExpenseInput input, CancellationToken cancellationToken);
}

