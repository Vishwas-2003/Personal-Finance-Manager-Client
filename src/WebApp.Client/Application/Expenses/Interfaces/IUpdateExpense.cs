namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IUpdateExpense
{
    Task ExecuteAsync(UpdateExpenseInput input, CancellationToken cancellationToken);
}
