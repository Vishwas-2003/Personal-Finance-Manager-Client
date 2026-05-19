using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Summary;

public sealed class GetExpenseSummary(ISummaryApi summaryApi, ISessionAccessor sessionAccessor) : IGetExpenseSummary
{
    public async Task<ExpenseSummary> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        return await summaryApi.GetExpenseSummaryAsync(session.UserId, cancellationToken);
    }
}
