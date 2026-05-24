using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Summary;

public sealed class GetIncomeSummary(ISummaryApi summaryApi, ISessionAccessor sessionAccessor) : IGetIncomeSummary
{
    public async Task<IncomeSummary> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        return await summaryApi.GetIncomeSummaryAsync(session.UserId, cancellationToken);
    }
}
