namespace WebApp.Client.Application.Income.Interfaces;

public interface IAddIncome
{
    Task ExecuteAsync(AddIncomeInput input, CancellationToken cancellationToken);
}

