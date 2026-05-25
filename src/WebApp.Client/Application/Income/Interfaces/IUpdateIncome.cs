namespace WebApp.Client.Application.Income.Interfaces;

public interface IUpdateIncome
{
    Task ExecuteAsync(UpdateIncomeInput input, CancellationToken cancellationToken);
}
