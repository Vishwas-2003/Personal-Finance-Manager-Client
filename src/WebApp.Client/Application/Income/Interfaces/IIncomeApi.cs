namespace WebApp.Client.Application.Income.Interfaces;

public interface IIncomeApi
{
    Task AddAsync(int userId, AddIncomeInput input, CancellationToken cancellationToken);
    Task UpdateAsync(int userId, UpdateIncomeInput input, CancellationToken cancellationToken);
    Task<IReadOnlyList<IncomeItem>> GetByUserIdAsync(int userId, IncomeListFilter? filter, CancellationToken cancellationToken);
    Task DeleteAsync(int incomeId, CancellationToken cancellationToken);
}
