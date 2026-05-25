using WebApp.Client.Application.Budget;
using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.ConsoleUi.Budget.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Budget;

public sealed class BudgetUi(
    IAddBudget addBudget,
    IListBudget listBudget,
    IDeleteBudget deleteBudget,
    IUpdateBudget updateBudget,
    IListCategories listCategories,
    IConsoleClearCoordinator consoleClearCoordinator) : IBudgetUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            PrintBudgetMenu();
            var choice = Input.RequiredMenuChoice(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.BudgetMenuMaxChoice,
                ClearAndRedrawBudgetMenu);

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
            else if (choice == AppConstants.Values.UpdateBudgetChoice)
            {
                await UpdateBudgetAsync();
            }
        }
    }

    private void PrintBudgetMenu()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.BudgetManagement, ConsoleMessageKind.Title);
        _console.WriteLine(AppConstants.Menus.Budget, ConsoleMessageKind.Menu);
    }

    private void ClearAndRedrawBudgetMenu()
    {
        consoleClearCoordinator.ClearScreen();
        PrintBudgetMenu();
    }

    private async Task AddBudgetAsync()
    {
        var categoryId = await ResolveCategoryIdAsync();
        var limitAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetLimitAmount, AppConstants.Values.MinBudgetLimitAmount);
        var spentAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetSpentAmount, AppConstants.Values.MinAmount);

        await addBudget.ExecuteAsync(new AddBudgetInput(categoryId, limitAmount, spentAmount), CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.BudgetAdded, ConsoleMessageKind.Success);
    }

    private async Task UpdateBudgetAsync()
    {
        var budgetId = Input.RequiredInt(AppConstants.Prompts.BudgetIdToUpdate, AppConstants.Values.MinBudgetId, int.MaxValue);
        var categoryId = await ResolveCategoryIdAsync();
        var limitAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetLimitAmount, AppConstants.Values.MinBudgetLimitAmount);
        var spentAmount = Input.RequiredDecimal(AppConstants.Prompts.BudgetSpentAmount, AppConstants.Values.MinAmount);

        await updateBudget.ExecuteAsync(
            new UpdateBudgetInput(budgetId, categoryId, limitAmount, spentAmount),
            CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.BudgetUpdated, ConsoleMessageKind.Success);
    }

    private async Task ListBudgetAsync()
    {
        var filter = await ReadListFilterAsync();
        var items = await listBudget.ExecuteAsync(filter, CancellationToken.None);
        if (items.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBudgets, ConsoleMessageKind.Muted);
            return;
        }

        var headers = new[] { "Id", "Limit", "Spent", "Category", "Type", "Updated (UTC)", "Status" };
        var rows = items.Select(budget => new[]
        {
            budget.Id.ToString(),
            NumberFormatUtility.FormatIndian(budget.LimitAmount),
            NumberFormatUtility.FormatIndian(budget.SpentAmount),
            budget.CategoryName,
            budget.CategoryType,
            budget.UpdatedAtUtc.ToUniversalTime().ToString("yyyy-MM-dd HH:mm'Z'"),
            budget.InActive ? AppConstants.Messages.StatusArchived : AppConstants.Messages.StatusActive
        });

        ConsoleTableUtility.Print(_console, headers, rows, row =>
            row[^1] == AppConstants.Messages.StatusArchived
                ? ConsoleMessageKind.Archived
                : ConsoleMessageKind.Data);
    }

    private async Task<BudgetListFilter?> ReadListFilterAsync()
    {
        if (Input.ShouldSkipAllFilters(AppConstants.Prompts.SkipAllFilters))
        {
            return null;
        }

        var categoryId = Input.OptionalInt(
            AppConstants.Prompts.ExpenseFilterCategoryOptional,
            AppConstants.Values.MinCategoryIdForViewOption);

        if (categoryId == 0)
        {
            await PrintCategoriesAsync();
            categoryId = Input.OptionalInt(
                AppConstants.Prompts.ExpenseFilterCategoryOptional,
                AppConstants.Values.MinCategoryId);
        }

        var keywords = Input.OptionalString(AppConstants.Prompts.KeywordFilterOptional);

        if (categoryId is null && string.IsNullOrWhiteSpace(keywords))
        {
            return null;
        }

        return new BudgetListFilter(categoryId, keywords);
    }

    private async Task DeleteBudgetAsync()
    {
        var budgetId = Input.RequiredInt(AppConstants.Prompts.BudgetIdToDelete, AppConstants.Values.MinBudgetId, int.MaxValue);
        await deleteBudget.ExecuteAsync(budgetId, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.BudgetDeleted, ConsoleMessageKind.Success);
    }

    private async Task<int> ResolveCategoryIdAsync()
    {
        var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinBudgetCategoryId, int.MaxValue);
        if (categoryId != 0)
        {
            return categoryId;
        }

        await PrintCategoriesAsync();
        return Input.RequiredInt(AppConstants.Prompts.CategoryId, AppConstants.Values.MinCategoryId, int.MaxValue);
    }

    private async Task PrintCategoriesAsync()
    {
        var categories = await listCategories.ExecuteAsync(CancellationToken.None);
        if (categories.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoCategoriesAvailable, ConsoleMessageKind.Muted);
            return;
        }

        _console.WriteLine(AppConstants.Messages.AvailableCategories, ConsoleMessageKind.Header);
        var headers = new[] { "Id", "Name", "Type" };
        var rows = categories.Select(category => new[]
        {
            category.Id.ToString(),
            category.Name,
            category.CategoryType
        });
        ConsoleTableUtility.Print(_console, headers, rows);
    }
}
