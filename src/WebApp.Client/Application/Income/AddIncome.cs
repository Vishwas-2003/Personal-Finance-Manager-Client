using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Income;

public sealed class AddIncome(IIncomeApi incomeApi, ISessionAccessor sessionAccessor) : IAddIncome
{
    public async Task ExecuteAsync(AddIncomeInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        await incomeApi.AddAsync(session.UserId, input, cancellationToken);
    }
}

