using WebApp.Client.Application.Budget;

namespace WebApp.Client.Application.Budget.Interfaces;

public interface IBudgetApi
{
    Task AddAsync(int userId, AddBudgetInput input, CancellationToken cancellationToken);
    Task<IReadOnlyList<BudgetItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task DeleteAsync(int budgetId, CancellationToken cancellationToken);
}
