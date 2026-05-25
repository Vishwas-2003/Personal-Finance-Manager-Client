using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Income;

public sealed class UpdateIncome(IIncomeApi incomeApi, ISessionAccessor sessionAccessor) : IUpdateIncome
{
    public async Task ExecuteAsync(UpdateIncomeInput input, CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new MustLoginException(AppConstants.Messages.MustLoginFirst);
        await incomeApi.UpdateAsync(session.UserId, input, cancellationToken);
    }
}
