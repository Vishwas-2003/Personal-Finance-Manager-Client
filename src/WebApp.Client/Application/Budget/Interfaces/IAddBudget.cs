using WebApp.Client.Application.Budget;

namespace WebApp.Client.Application.Budget.Interfaces;

public interface IAddBudget
{
    Task ExecuteAsync(AddBudgetInput input, CancellationToken cancellationToken);
}
