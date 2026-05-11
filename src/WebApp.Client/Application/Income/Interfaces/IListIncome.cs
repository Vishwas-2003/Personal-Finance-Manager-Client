namespace WebApp.Client.Application.Income.Interfaces;

public interface IListIncome
{
    Task<IReadOnlyList<IncomeItem>> ExecuteAsync(CancellationToken cancellationToken);
}

