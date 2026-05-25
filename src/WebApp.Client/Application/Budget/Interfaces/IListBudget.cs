using WebApp.Client.Application.Budget;

namespace WebApp.Client.Application.Budget.Interfaces;

public interface IListBudget
{
    Task<IReadOnlyList<BudgetItem>> ExecuteAsync(BudgetListFilter? filter, CancellationToken cancellationToken);
}
