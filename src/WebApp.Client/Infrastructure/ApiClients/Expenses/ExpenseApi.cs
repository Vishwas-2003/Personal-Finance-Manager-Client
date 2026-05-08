using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Expenses;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Expenses;

public sealed class ExpenseApi(IHttpClientFactory httpClientFactory) : IExpenseApi
{
    public async Task AddAsync(int userId, AddExpenseInput input, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var request = new AddExpenseRequestModel
        {
            UserId = userId,
            Amount = input.Amount,
            CategoryId = input.CategoryId,
            Description = input.Description,
            Date = input.Date
        };
        var response = await client.PostAsync(
            RouteConstants.Expense.Add,
            JsonHttp.CreateBody(request),
            cancellationToken);

        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<ExpenseItem>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Expense.GetByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<List<ExpenseResponseModel>>(response, cancellationToken);
        return payload.Select(ToItem).ToArray();
    }

    public async Task DeleteAsync(int expenseId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Expense.DeleteById.Replace(AppConstants.RoutePlaceholders.ExpenseId, expenseId.ToString());
        var response = await client.DeleteAsync(path, cancellationToken);
        await JsonHttp.EnsureSuccessAsync(response, cancellationToken);
    }

    private static ExpenseItem ToItem(ExpenseResponseModel r) =>
        new(
            r.Id,
            r.Amount,
            r.Category.Id,
            r.Category.Name,
            r.Category.CategoryType,
            r.Description,
            r.Date,
            r.CreatedAtUtc);
}

