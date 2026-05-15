using WebApp.Client.Application.Summary;

namespace WebApp.Client.Application.Summary.Interfaces;

public interface IGetIncomeSummary
{
    Task<IncomeSummary> ExecuteAsync(CancellationToken cancellationToken);
}
