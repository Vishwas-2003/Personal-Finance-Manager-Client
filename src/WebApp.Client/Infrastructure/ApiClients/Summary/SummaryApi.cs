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
            payload.CategoryTypeSections.Select(ToIncomeCategoryTypeSection).ToArray(),
            payload.TotalIncome);
    }

    public async Task<ExpenseSummary> GetExpenseSummaryAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Summary.ExpenseByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<ExpenseSummaryResponseModel>(response, cancellationToken);
        return new ExpenseSummary(
            payload.CategoryTypeSections.Select(ToExpenseCategoryTypeSection).ToArray(),
            payload.TotalExpense);
    }

    public async Task<BalanceSummary> GetBalanceSummaryAsync(int userId, BalanceSummaryFilter? filter, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Summary.BalanceByUserId.Replace(AppConstants.RoutePlaceholders.UserId, userId.ToString());
        path = AppendFilterQuery(path, filter);
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<BalanceSummaryResponseModel>(response, cancellationToken);
        return new BalanceSummary(
            payload.Credits.Select(ToCreditLine).ToArray(),
            payload.Debits.Select(ToDebitLine).ToArray(),
            payload.TotalCredit,
            payload.TotalDebit,
            payload.Balance);
    }

    private static string AppendFilterQuery(string path, BalanceSummaryFilter? filter)
    {
        if (filter is null)
        {
            return path;
        }

        var queryParts = new List<string>();
        if (filter.FromDate is DateTime fromDate)
        {
            queryParts.Add($"filter.FromDate={Uri.EscapeDataString(fromDate.ToString(AppConstants.Formats.Date))}");
        }

        if (filter.ToDate is DateTime toDate)
        {
            queryParts.Add($"filter.ToDate={Uri.EscapeDataString(toDate.ToString(AppConstants.Formats.Date))}");
        }

        return queryParts.Count == 0 ? path : $"{path}?{string.Join("&", queryParts)}";
    }

    private static BalanceSummaryCreditLine ToCreditLine(BalanceSummaryCreditLineModel line) =>
        new(line.Id, line.Amount, line.Date, line.Source, line.Notes, line.CategoryName, line.CategoryType);

    private static BalanceSummaryDebitLine ToDebitLine(BalanceSummaryDebitLineModel line) =>
        new(line.Id, line.Amount, line.Date, line.Description, line.CategoryName, line.CategoryType);

    private static IncomeSummaryCategoryTypeSection ToIncomeCategoryTypeSection(
        IncomeSummaryCategoryTypeSectionModel section) =>
        new(
            section.CategoryTypeId,
            section.CategoryTypeName,
            section.SectionTotal,
            section.SubCategorySections.Select(ToIncomeSubCategorySection).ToArray());

    private static IncomeSummarySubCategorySection ToIncomeSubCategorySection(
        IncomeSummarySubCategorySectionModel section) =>
        new(
            section.CategoryId,
            section.CategoryName,
            section.Subtotal,
            section.IncomeEntries.Select(ToIncomeItem).ToArray());

    private static ExpenseSummaryCategoryTypeSection ToExpenseCategoryTypeSection(
        ExpenseSummaryCategoryTypeSectionModel section) =>
        new(
            section.CategoryTypeId,
            section.CategoryTypeName,
            section.SectionTotal,
            section.SubCategorySections.Select(ToExpenseSubCategorySection).ToArray());

    private static ExpenseSummarySubCategorySection ToExpenseSubCategorySection(
        ExpenseSummarySubCategorySectionModel section) =>
        new(
            section.CategoryId,
            section.CategoryName,
            section.Subtotal,
            section.ExpenseEntries.Select(ToExpenseItem).ToArray());

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
            income.CreatedAtUtc,
            income.InActive);

    private static ExpenseItem ToExpenseItem(ExpenseResponseModel expense) =>
        new(
            expense.Id,
            expense.Amount,
            expense.Category.Id,
            expense.Category.Name,
            expense.Category.CategoryType,
            expense.Description,
            expense.Date,
            expense.CreatedAtUtc,
            expense.InActive);
}
