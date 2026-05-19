using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Summary;

public sealed class GetBalanceSummary(ISummaryApi summaryApi, ISessionAccessor sessionAccessor) : IGetBalanceSummary
{
    public async Task<BalanceSummary> ExecuteAsync(BalanceSummaryFilter? filter, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        return await summaryApi.GetBalanceSummaryAsync(session.UserId, filter, cancellationToken);
    }
}
