using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Expenses;

public sealed class AddExpense(IExpenseApi expenseApi, ISessionAccessor sessionAccessor) : IAddExpense
{
    public async Task ExecuteAsync(AddExpenseInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        await expenseApi.AddAsync(session.UserId, input, cancellationToken);
    }
}
