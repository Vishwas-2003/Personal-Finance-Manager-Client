using WebApp.Client.Application.Budget.Interfaces;

namespace WebApp.Client.Application.Budget;

public sealed class DeleteBudget(IBudgetApi budgetApi) : IDeleteBudget
{
    public Task ExecuteAsync(int budgetId, CancellationToken cancellationToken) =>
        budgetApi.DeleteAsync(budgetId, cancellationToken);
}
