namespace WebApp.Client.Application.Income.Interfaces;

public interface IIncomeApi
{
    Task AddAsync(int userId, AddIncomeInput input, CancellationToken cancellationToken);
    Task<IReadOnlyList<IncomeItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task DeleteAsync(int incomeId, CancellationToken cancellationToken);
}

