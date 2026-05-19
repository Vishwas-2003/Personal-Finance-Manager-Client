using WebApp.Client.Application.Summary;

namespace WebApp.Client.Application.Summary.Interfaces;

public interface ISummaryApi
{
    Task<IncomeSummary> GetIncomeSummaryAsync(int userId, CancellationToken cancellationToken);
    Task<ExpenseSummary> GetExpenseSummaryAsync(int userId, CancellationToken cancellationToken);
    Task<BalanceSummary> GetBalanceSummaryAsync(int userId, BalanceSummaryFilter? filter, CancellationToken cancellationToken);
}
