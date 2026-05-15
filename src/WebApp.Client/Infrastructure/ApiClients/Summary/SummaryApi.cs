using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Income;
using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Expenses;
using WebApp.Client.Infrastructure.Contracts.Income;
using WebApp.Client.Infrastructure.Contracts.Summary;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Summary;

public sealed class SummaryApi(IHttpClientFactory httpClientFactory) : ISummaryApi
{
    public async Task<IncomeSummary> GetIncomeSummaryAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Summary.IncomeByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<IncomeSummaryResponseModel>(response, cancellationToken);
        return new IncomeSummary(
            payload.CategoryTypes.Select(ToIncomeTypeGroup).ToArray(),
            payload.TotalIncome);
    }

    public async Task<ExpenseSummary> GetExpenseSummaryAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Summary.ExpenseByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<ExpenseSummaryResponseModel>(response, cancellationToken);
        return new ExpenseSummary(
            payload.CategoryTypes.Select(ToExpenseTypeGroup).ToArray(),
            payload.TotalExpense);
    }

    private static IncomeSummaryCategoryTypeGroup ToIncomeTypeGroup(IncomeSummaryCategoryTypeGroupModel group) =>
        new(
            group.CategoryTypeId,
            group.CategoryType,
            group.Subtotal,
            group.Categories.Select(ToIncomeCategoryGroup).ToArray());

    private static IncomeSummaryCategoryGroup ToIncomeCategoryGroup(IncomeSummaryCategoryGroupModel group) =>
        new(
            group.CategoryId,
            group.CategoryName,
            group.Subtotal,
            group.Items.Select(ToIncomeItem).ToArray());

    private static ExpenseSummaryCategoryTypeGroup ToExpenseTypeGroup(ExpenseSummaryCategoryTypeGroupModel group) =>
        new(
            group.CategoryTypeId,
            group.CategoryType,
            group.Subtotal,
            group.Categories.Select(ToExpenseCategoryGroup).ToArray());

    private static ExpenseSummaryCategoryGroup ToExpenseCategoryGroup(ExpenseSummaryCategoryGroupModel group) =>
        new(
            group.CategoryId,
            group.CategoryName,
            group.Subtotal,
            group.Items.Select(ToExpenseItem).ToArray());

    private static IncomeItem ToIncomeItem(IncomeResponseModel income) =>
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

    private static ExpenseItem ToExpenseItem(ExpenseResponseModel expense) =>
        new(
            expense.Id,
            expense.Amount,
            expense.Category.Id,
            expense.Category.Name,
            expense.Category.CategoryType,
            expense.Description,
            expense.Date,
            expense.CreatedAtUtc);
}
