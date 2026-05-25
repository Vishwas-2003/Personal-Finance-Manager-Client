using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Expenses;

public sealed class UpdateExpense(IExpenseApi expenseApi, ISessionAccessor sessionAccessor) : IUpdateExpense
{
    public async Task ExecuteAsync(UpdateExpenseInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        await expenseApi.UpdateAsync(session.UserId, input, cancellationToken);
    }
}
