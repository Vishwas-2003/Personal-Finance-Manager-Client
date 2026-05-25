using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.ConsoleUi.Expense.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Expense;

public class ExpenseUi(
    IAddExpense addExpense,
    IListExpenses listExpenses,
    IDeleteExpense deleteExpense,
    IUpdateExpense updateExpense,
    IListCategories listCategories,
    IConsoleClearCoordinator consoleClearCoordinator)
    : IExpenseUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        await ExpensesMenuAsync();
    }

    private async Task ExpensesMenuAsync()
    {
        while (true)
        {
            PrintExpenseMenu();
            var choice = Input.RequiredMenuChoice(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.ExpenseMenuMaxChoice,
                ClearAndRedrawExpenseMenu);

            if (choice == 0)
            {
                return;
            }

            if (choice == AppConstants.Values.AddExpenseChoice)
            {
                await AddExpense();
            }
            else if (choice == AppConstants.Values.ListExpenseChoice)
            {
                await ListExpense();
            }
            else if (choice == AppConstants.Values.DeleteExpenseChoice)
            {
                await DeleteExpense();
            }
            else if (choice == AppConstants.Values.UpdateExpenseChoice)
            {
                await UpdateExpenseAsync();
            }
        }
    }

    private void PrintExpenseMenu()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.ExpenseManagement, ConsoleMessageKind.Title);
        _console.WriteLine(AppConstants.Menus.Expense, ConsoleMessageKind.Menu);
    }

    private void ClearAndRedrawExpenseMenu()
    {
        consoleClearCoordinator.ClearScreen();
        PrintExpenseMenu();
    }

    private async Task AddExpense()
    {
        var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
        var categoryId = await ResolveCategoryIdAsync();

        var description = Input.OptionalString(AppConstants.Prompts.DescriptionOptional);
        var date = Input.RequiredDate(AppConstants.Prompts.Date, disallowFuture: true);
        await addExpense.ExecuteAsync(new AddExpenseInput(amount, categoryId, description, date), CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.ExpenseAdded, ConsoleMessageKind.Success);
    }

    private async Task UpdateExpenseAsync()
    {
        var expenseId = Input.RequiredInt(AppConstants.Prompts.ExpenseIdToUpdate, AppConstants.Values.MinExpenseId, int.MaxValue);
        var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
        var categoryId = await ResolveCategoryIdAsync();
        var description = Input.OptionalString(AppConstants.Prompts.DescriptionOptional);
        var date = Input.RequiredDate(AppConstants.Prompts.Date, disallowFuture: true);

        await updateExpense.ExecuteAsync(
            new UpdateExpenseInput(expenseId, amount, categoryId, description, date),
            CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.ExpenseUpdated, ConsoleMessageKind.Success);
    }

    private async Task ListExpense()
    {
        var filter = await ReadListFilterAsync();
        var items = await listExpenses.ExecuteAsync(filter, CancellationToken.None);
        if (items.Count == 0)
        {
            _console.WriteLine(
                filter is null ? AppConstants.Messages.NoExpenses : AppConstants.Messages.NoExpensesForFilter,
                ConsoleMessageKind.Muted);
            return;
        }

        var headers = new[] { "Id", "Amount", "Category", "Type", "Date", "Description", "Status" };
        var rows = items.Select(e => new[]
        {
            e.Id.ToString(),
            NumberFormatUtility.FormatIndian(e.Amount),
            e.CategoryName,
            e.CategoryType,
            e.Date.ToString(AppConstants.Formats.Date),
            e.Description ?? string.Empty,
            e.InActive ? AppConstants.Messages.StatusArchived : AppConstants.Messages.StatusActive
        });

        ConsoleTableUtility.Print(_console, headers, rows, row =>
            row[^1] == AppConstants.Messages.StatusArchived
                ? ConsoleMessageKind.Archived
                : ConsoleMessageKind.Data);
    }

    private async Task<ExpenseListFilter?> ReadListFilterAsync()
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

        return new ExpenseListFilter(categoryId, fromDate, toDate, keywords);
    }

    private async Task DeleteExpense()
    {
        var expenseId = Input.RequiredInt(AppConstants.Prompts.ExpenseIdToDelete, AppConstants.Values.MinExpenseId, int.MaxValue);
        await deleteExpense.ExecuteAsync(expenseId, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.ExpenseDeleted, ConsoleMessageKind.Success);
    }

    private async Task<int> ResolveCategoryIdAsync()
    {
        var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinCategoryIdForViewOption, int.MaxValue);
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
