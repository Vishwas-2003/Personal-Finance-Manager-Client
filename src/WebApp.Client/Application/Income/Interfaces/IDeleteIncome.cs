namespace WebApp.Client.Application.Income.Interfaces;

public interface IDeleteIncome
{
    Task ExecuteAsync(int incomeId, CancellationToken cancellationToken);
}

