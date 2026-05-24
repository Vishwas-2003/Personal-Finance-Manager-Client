using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Expenses;

public sealed class ListExpenses(IExpenseApi expenseApi, ISessionAccessor sessionAccessor) : IListExpenses
{
    public async Task<IReadOnlyList<ExpenseItem>> ExecuteAsync(ExpenseListFilter? filter, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        return await expenseApi.GetByUserIdAsync(session.UserId, filter, cancellationToken);
    }
}

