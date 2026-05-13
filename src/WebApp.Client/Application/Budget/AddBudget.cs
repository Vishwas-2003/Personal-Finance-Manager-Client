using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Budget;

public sealed class AddBudget(IBudgetApi budgetApi, ISessionAccessor sessionAccessor) : IAddBudget
{
    public async Task ExecuteAsync(AddBudgetInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        await budgetApi.AddAsync(session.UserId, input, cancellationToken);
    }
}
