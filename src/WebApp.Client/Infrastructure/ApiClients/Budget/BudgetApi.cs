using WebApp.Client.Application.Budget;
using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Budget;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Budget;

public sealed class BudgetApi(IHttpClientFactory httpClientFactory) : IBudgetApi
{
    public async Task AddAsync(int userId, AddBudgetInput input, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var request = new AddBudgetRequestModel
        {
            UserId = userId,
            CategoryId = input.CategoryId,
            LimitAmount = input.LimitAmount,
            SpentAmount = input.SpentAmount
        };

        var response = await client.PostAsync(
            RouteConstants.Budget.Add,
            JsonHttp.CreateBody(request),
            cancellationToken);

        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task UpdateAsync(int userId, UpdateBudgetInput input, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Budget.UpdateById.Replace(AppConstants.RoutePlaceholders.BudgetId, input.Id.ToString());
        var request = new AddBudgetRequestModel
        {
            UserId = userId,
            CategoryId = input.CategoryId,
            LimitAmount = input.LimitAmount,
            SpentAmount = input.SpentAmount
        };
        var response = await client.PutAsync(path, JsonHttp.CreateBody(request), cancellationToken);
        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<BudgetItem>> GetByUserIdAsync(int userId, BudgetListFilter? filter, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Budget.GetByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        path = AppendFilterQuery(path, filter);
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<List<BudgetResponseModel>>(response, cancellationToken);
        return payload.Select(ToItem).ToArray();
    }

    public async Task DeleteAsync(int budgetId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Budget.DeleteById.Replace(AppConstants.RoutePlaceholders.BudgetId, budgetId.ToString());
        var response = await client.DeleteAsync(path, cancellationToken);
        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    private static string AppendFilterQuery(string path, BudgetListFilter? filter)
    {
        if (filter is null)
        {
            return path;
        }

        var queryParts = new List<string>();
        if (filter.CategoryId is int categoryId)
        {
            queryParts.Add($"filter.CategoryId={categoryId}");
        }

        if (!string.IsNullOrWhiteSpace(filter.Keywords))
        {
            queryParts.Add($"filter.Keywords={Uri.EscapeDataString(filter.Keywords)}");
        }

        return queryParts.Count == 0 ? path : $"{path}?{string.Join("&", queryParts)}";
    }

    private static BudgetItem ToItem(BudgetResponseModel budget) =>
        new(
            budget.Id,
            budget.LimitAmount,
            budget.SpentAmount,
            budget.UpdatedAtUtc,
            budget.Category.Id,
            budget.Category.Name,
            budget.Category.CategoryType,
            budget.InActive);
}
