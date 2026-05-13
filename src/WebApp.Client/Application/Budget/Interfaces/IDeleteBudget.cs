namespace WebApp.Client.Application.Budget.Interfaces;

public interface IDeleteBudget
{
    Task ExecuteAsync(int budgetId, CancellationToken cancellationToken);
}
