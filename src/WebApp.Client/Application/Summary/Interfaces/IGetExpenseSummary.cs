using WebApp.Client.Application.Summary;

namespace WebApp.Client.Application.Summary.Interfaces;

public interface IGetExpenseSummary
{
    Task<ExpenseSummary> ExecuteAsync(CancellationToken cancellationToken);
}
