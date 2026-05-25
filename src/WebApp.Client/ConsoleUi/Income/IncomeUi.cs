using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Application.Income;
using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.ConsoleUi.Income.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Income;

public sealed class IncomeUi(
    IAddIncome addIncome,
    IListIncome listIncome,
    IDeleteIncome deleteIncome,
    IUpdateIncome updateIncome,
    IListCategories listCategories,
    IConsoleClearCoordinator consoleClearCoordinator) : IIncomeUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            PrintIncomeMenu();
            var choice = Input.RequiredMenuChoice(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.IncomeMenuMaxChoice,
                ClearAndRedrawIncomeMenu);

            if (choice == AppConstants.Values.MenuMinChoice)
            {
                return;
            }

            if (choice == AppConstants.Values.AddIncomeChoice)
            {
                await AddIncomeAsync();
            }
            else if (choice == AppConstants.Values.ListIncomeChoice)
            {
                await ListIncomeAsync();
            }
            else if (choice == AppConstants.Values.DeleteIncomeChoice)
            {
                await DeleteIncomeAsync();
            }
            else if (choice == AppConstants.Values.UpdateIncomeChoice)
            {
                await UpdateIncomeAsync();
            }
        }
    }

    private void PrintIncomeMenu()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.IncomeManagement, ConsoleMessageKind.Title);
        _console.WriteLine(AppConstants.Menus.Income, ConsoleMessageKind.Menu);
    }

    private void ClearAndRedrawIncomeMenu()
    {
        consoleClearCoordinator.ClearScreen();
        PrintIncomeMenu();
    }

    private async Task AddIncomeAsync()
    {
        var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
        var categoryId = await ResolveCategoryIdAsync();
        var date = Input.RequiredDate(AppConstants.Prompts.Date, disallowFuture: true);
        var source = Input.RequiredString(AppConstants.Prompts.IncomeSource);
        var notes = Input.OptionalString(AppConstants.Prompts.IncomeNotesOptional);

        await addIncome.ExecuteAsync(new AddIncomeInput(amount, categoryId, date, source, notes), CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.IncomeAdded, ConsoleMessageKind.Success);
    }

    private async Task UpdateIncomeAsync()
    {
        var incomeId = Input.RequiredInt(AppConstants.Prompts.IncomeIdToUpdate, AppConstants.Values.MinIncomeId, int.MaxValue);
        var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
        var categoryId = await ResolveCategoryIdAsync();
        var date = Input.RequiredDate(AppConstants.Prompts.Date, disallowFuture: true);
        var source = Input.RequiredString(AppConstants.Prompts.IncomeSource);
        var notes = Input.OptionalString(AppConstants.Prompts.IncomeNotesOptional);

        await updateIncome.ExecuteAsync(
            new UpdateIncomeInput(incomeId, amount, categoryId, date, source, notes),
            CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.IncomeUpdated, ConsoleMessageKind.Success);
    }

    private async Task ListIncomeAsync()
    {
        var filter = await ReadListFilterAsync();
        var items = await listIncome.ExecuteAsync(filter, CancellationToken.None);
        if (items.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoIncome, ConsoleMessageKind.Muted);
            return;
        }

        var headers = new[] { "Id", "Amount", "Category", "Type", "Date", "Source", "Notes", "Status" };
        var rows = items.Select(income => new[]
        {
            income.Id.ToString(),
            NumberFormatUtility.FormatIndian(income.Amount),
            income.CategoryName,
            income.CategoryType,
            income.Date.ToString(AppConstants.Formats.Date),
            income.Source,
            income.Notes ?? string.Empty,
            income.InActive ? AppConstants.Messages.StatusArchived : AppConstants.Messages.StatusActive
        });

        ConsoleTableUtility.Print(_console, headers, rows, row =>
            row[^1] == AppConstants.Messages.StatusArchived
                ? ConsoleMessageKind.Archived
                : ConsoleMessageKind.Data);
    }

    private async Task<IncomeListFilter?> ReadListFilterAsync()
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

        DateTime? fromDate;
        DateTime? toDate;
        do
        {
            fromDate = Input.OptionalDate(AppConstants.Prompts.ExpenseFilterFromDateOptional);
            toDate = Input.OptionalDate(AppConstants.Prompts.ExpenseFilterToDateOptional);
            if (fromDate is not null && toDate is not null && fromDate.Value.Date > toDate.Value.Date)
            {
                _console.WriteLine(AppConstants.Messages.InvalidDateRange, ConsoleMessageKind.Warning);
            }
            else
            {
                break;
            }
        }
        while (true);

        var keywords = Input.OptionalString(AppConstants.Prompts.KeywordFilterOptional);

        if (categoryId is null && fromDate is null && toDate is null && string.IsNullOrWhiteSpace(keywords))
        {
            return null;
        }

        return new IncomeListFilter(categoryId, fromDate, toDate, keywords);
    }

    private async Task DeleteIncomeAsync()
    {
        var incomeId = Input.RequiredInt(AppConstants.Prompts.IncomeIdToDelete, AppConstants.Values.MinIncomeId, int.MaxValue);
        await deleteIncome.ExecuteAsync(incomeId, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.IncomeDeleted, ConsoleMessageKind.Success);
    }

    private async Task<int> ResolveCategoryIdAsync()
    {
        var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinIncomeCategoryId, int.MaxValue);
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
