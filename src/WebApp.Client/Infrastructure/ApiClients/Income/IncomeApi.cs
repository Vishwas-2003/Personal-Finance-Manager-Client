using WebApp.Client.Application.Income;
using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Income;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Income;

public sealed class IncomeApi(IHttpClientFactory httpClientFactory) : IIncomeApi
{
    public async Task AddAsync(int userId, AddIncomeInput input, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var request = new AddIncomeRequestModel
        {
            UserId = userId,
            Amount = input.Amount,
            CategoryId = input.CategoryId,
            Date = input.Date,
            Source = input.Source,
            Notes = input.Notes
        };

        var response = await client.PostAsync(
            RouteConstants.Income.Add,
            JsonHttp.CreateBody(request),
            cancellationToken);

        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<IncomeItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Income.GetByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<List<IncomeResponseModel>>(response, cancellationToken);
        return payload.Select(ToItem).ToArray();
    }

    public async Task DeleteAsync(int incomeId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Income.DeleteById.Replace(AppConstants.RoutePlaceholders.IncomeId, incomeId.ToString());
        var response = await client.DeleteAsync(path, cancellationToken);
        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    private static IncomeItem ToItem(IncomeResponseModel income) =>
        new(
            income.Id,
            income.Amount,
            income.Category.Id,
            income.Category.Name,
            income.Category.CategoryType,
            income.Date,
            income.Source,
            income.Notes,
            income.CreatedAtUtc);
}

