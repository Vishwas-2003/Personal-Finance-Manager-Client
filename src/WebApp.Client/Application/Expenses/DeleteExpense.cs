using WebApp.Client.Application.Expenses.Interfaces;

namespace WebApp.Client.Application.Expenses;

public sealed class DeleteExpense(IExpenseApi expenseApi) : IDeleteExpense
{
    public Task ExecuteAsync(int expenseId, CancellationToken cancellationToken) =>
        expenseApi.DeleteAsync(expenseId, cancellationToken);
}

