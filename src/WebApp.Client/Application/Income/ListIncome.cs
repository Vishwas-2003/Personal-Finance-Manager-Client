using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Income;

public sealed class ListIncome(IIncomeApi incomeApi, ISessionAccessor sessionAccessor) : IListIncome
{
    public async Task<IReadOnlyList<IncomeItem>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        return await incomeApi.GetByUserIdAsync(session.UserId, cancellationToken);
    }
}

