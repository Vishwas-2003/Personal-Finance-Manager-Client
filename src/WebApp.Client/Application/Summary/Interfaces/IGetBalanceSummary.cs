namespace WebApp.Client.Application.Summary.Interfaces;

public interface IGetBalanceSummary
{
    Task<BalanceSummary> ExecuteAsync(BalanceSummaryFilter? filter, CancellationToken cancellationToken);
}
