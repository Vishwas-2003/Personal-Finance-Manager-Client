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
    IListCategories listCategories) : IIncomeUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            _console.WriteLine(string.Empty);
            _console.WriteLine(AppConstants.Titles.IncomeManagement);
            _console.WriteLine(AppConstants.Menus.Income);
            var choice = Input.RequiredInt(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.IncomeMenuMaxChoice);

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
        }
    }

    private async Task AddIncomeAsync()
    {
        var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
        var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinIncomeCategoryId, int.MaxValue);

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

        var date = Input.RequiredDate(AppConstants.Prompts.Date);
        var source = Input.RequiredString(AppConstants.Prompts.IncomeSource);
        var notes = Input.OptionalString(AppConstants.Prompts.IncomeNotesOptional);

        await addIncome.ExecuteAsync(new AddIncomeInput(amount, categoryId, date, source, notes), CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.IncomeAdded);
    }

    private async Task ListIncomeAsync()
    {
        var items = await listIncome.ExecuteAsync(CancellationToken.None);
        if (items.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoIncome);
            return;
        }

        _console.WriteLine(AppConstants.Messages.IncomeHeader);
        foreach (var income in items.OrderByDescending(x => x.Date))
        {
            _console.WriteLine(string.Format(
                AppConstants.Messages.IncomeRowFormat,
                income.Id,
                NumberFormatUtility.FormatIndian(income.Amount),
                income.CategoryName,
                income.CategoryType,
                income.Date.ToString(AppConstants.Formats.Date),
                income.Source,
                income.Notes));
        }
    }

    private async Task DeleteIncomeAsync()
    {
        var incomeId = Input.RequiredInt(AppConstants.Prompts.IncomeIdToDelete, AppConstants.Values.MinIncomeId, int.MaxValue);
        await deleteIncome.ExecuteAsync(incomeId, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.IncomeDeleted);
    }
}

