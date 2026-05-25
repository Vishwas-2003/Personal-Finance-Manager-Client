using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Budget;

public sealed class UpdateBudget(IBudgetApi budgetApi, ISessionAccessor sessionAccessor) : IUpdateBudget
{
    public async Task ExecuteAsync(UpdateBudgetInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        await budgetApi.UpdateAsync(session.UserId, input, cancellationToken);
    }
}
