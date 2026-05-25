using WebApp.Client.Application.Budget;

namespace WebApp.Client.Application.Budget.Interfaces;

public interface IUpdateBudget
{
    Task ExecuteAsync(UpdateBudgetInput input, CancellationToken cancellationToken);
}
