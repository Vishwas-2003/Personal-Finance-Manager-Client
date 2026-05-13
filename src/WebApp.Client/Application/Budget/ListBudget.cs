using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Budget;

public sealed class ListBudget(IBudgetApi budgetApi, ISessionAccessor sessionAccessor) : IListBudget
{
    public async Task<IReadOnlyList<BudgetItem>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        return await budgetApi.GetByUserIdAsync(session.UserId, cancellationToken);
    }
}
