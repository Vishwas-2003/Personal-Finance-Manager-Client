using WebApp.Client.Application.Income.Interfaces;

namespace WebApp.Client.Application.Income;

public sealed class DeleteIncome(IIncomeApi incomeApi) : IDeleteIncome
{
    public Task ExecuteAsync(int incomeId, CancellationToken cancellationToken) =>
        incomeApi.DeleteAsync(incomeId, cancellationToken);
}

