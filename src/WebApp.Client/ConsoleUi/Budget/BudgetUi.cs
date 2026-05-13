using WebApp.Client.Application.Budget;
using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.ConsoleUi.Budget.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;

namespace WebApp.Client.ConsoleUi.Budget;

public sealed class BudgetUi(
    IAddBudget addBudget,
    IListBudget listBudget,
    IDeleteBudget deleteBudget,
    IListCategories listCategories) : IBudgetUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            _console.WriteLine(string.Empty);
            _console.WriteLine(AppConstants.Titles.BudgetManagement);
            _console.WriteLine(AppConstants.Menus.Budget);
            var choice = Input.RequiredInt(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.BudgetMenuMaxChoice);

            if (choice == AppConstants.Values.MenuMinChoice)
            {
                return;
            }

            if (choice == AppConstants.Values.AddBudgetChoice)
            {
                await AddBudgetAsync();
            }
            else if (choice == AppConstants.Values.ListBudgetChoice)
            {
                await ListBudgetAsync();
            }
            else if (choice == AppConstants.Values.DeleteBudgetChoice)
            {
                await DeleteBudgetAsync();
            }
        }
    }

    private async Task AddBudgetAsync()
    {
        var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinBudgetCategoryId, int.MaxValue);

        if (categoryId == 0)
        {
            var categories = await listCategories.ExecuteAsync(CancellationToken.None);
            if (categories.Any())
            {
                _console.WriteLine(AppConstants.Messages.AvailableCategories);
                _console.WriteLine(AppConstants.Messages.AvailableCategoriesHeader);

                foreach (var category in categories)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.CategoriesRowFormat,
                        category.Id,
                        category.Name,
                        category.CategoryType));
                }
            }
            else
            {
                _console.WriteLine(AppConstants.Messages.NoCategoriesAvailable);
            }

            categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryId, AppConstants.Values.MinCategoryId, int.MaxValue);
        }

        var limitAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetLimitAmount, AppConstants.Values.MinBudgetLimitAmount);
        var spentAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetSpentAmount, AppConstants.Values.MinAmount);

        await addBudget.ExecuteAsync(new AddBudgetInput(categoryId, limitAmount, spentAmount), CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.BudgetAdded);
    }

    private async Task ListBudgetAsync()
    {
        var items = await listBudget.ExecuteAsync(CancellationToken.None);
        if (items.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBudgets);
            return;
        }

        _console.WriteLine(AppConstants.Messages.BudgetHeader);
        foreach (var budget in items.OrderByDescending(x => x.UpdatedAtUtc))
        {
            _console.WriteLine(string.Format(
                AppConstants.Messages.BudgetRowFormat,
                budget.Id,
                budget.LimitAmount,
                budget.SpentAmount,
                budget.CategoryName,
                budget.CategoryType,
                budget.UpdatedAtUtc.ToUniversalTime().ToString("yyyy-MM-dd HH:mm'Z'")));
        }
    }

    private async Task DeleteBudgetAsync()
    {
        var budgetId = Input.RequiredInt(AppConstants.Prompts.BudgetIdToDelete, AppConstants.Values.MinBudgetId, int.MaxValue);
        await deleteBudget.ExecuteAsync(budgetId, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.BudgetDeleted);
    }
}
