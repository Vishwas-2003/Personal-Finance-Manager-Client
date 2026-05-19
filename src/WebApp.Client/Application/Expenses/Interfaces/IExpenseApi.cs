namespace WebApp.Client.Application.Expenses.Interfaces;

public interface IExpenseApi
{
    Task AddAsync(int userId, AddExpenseInput input, CancellationToken cancellationToken);
    Task<IReadOnlyList<ExpenseItem>> GetByUserIdAsync(int userId, ExpenseListFilter? filter, CancellationToken cancellationToken);
    Task DeleteAsync(int expenseId, CancellationToken cancellationToken);
}

