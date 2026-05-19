using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.ConsoleUi.Expense.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Expense
{
    public class ExpenseUi(
        IAddExpense addExpense,
        IListExpenses listExpenses,
        IDeleteExpense deleteExpense,
        IListCategories listCategories)
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
                _console.WriteLine(string.Empty);
                _console.WriteLine(AppConstants.Titles.ExpenseManagement);
                _console.WriteLine(AppConstants.Menus.Expense);
                var choice = Input.RequiredInt(AppConstants.Prompts.ChooseOption, AppConstants.Values.MenuMinChoice, AppConstants.Values.ExpenseMenuMaxChoice);

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
            }
        }

        private async Task AddExpense()
        {
            var amount = Input.RequiredDecimal(AppConstants.Prompts.Amount, AppConstants.Values.MinAmount);
            var categoryId = Input.RequiredInt(AppConstants.Prompts.CategoryIdWithViewOption, AppConstants.Values.MinCategoryIdForViewOption, int.MaxValue);

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

            var description = Input.OptionalString(AppConstants.Prompts.DescriptionOptional);
            var date = Input.RequiredDate(AppConstants.Prompts.Date);
            await addExpense.ExecuteAsync(new AddExpenseInput(amount, categoryId, description, date), CancellationToken.None);
            _console.WriteLine(AppConstants.Messages.ExpenseAdded);
        }

        private async Task ListExpense()
        {
            var filter = await ReadListFilterAsync();
            var items = await listExpenses.ExecuteAsync(filter, CancellationToken.None);
            if (items.Count == 0)
            {
                _console.WriteLine(filter is null ? AppConstants.Messages.NoExpenses : AppConstants.Messages.NoExpensesForFilter);
                return;
            }

            _console.WriteLine(AppConstants.Messages.ExpenseHeader);
            foreach (var e in items.OrderByDescending(x => x.Date))
            {
                _console.WriteLine(string.Format(
                    AppConstants.Messages.ExpenseRowFormat,
                    e.Id,
                    NumberFormatUtility.FormatIndian(e.Amount),
                    e.CategoryName,
                    e.CategoryType,
                    e.Date.ToString(AppConstants.Formats.Date),
                    e.Description));
            }
        }

        private async Task<ExpenseListFilter?> ReadListFilterAsync()
        {
            var categoryId = Input.OptionalInt(
                AppConstants.Prompts.ExpenseFilterCategoryOptional,
                AppConstants.Values.MinCategoryIdForViewOption);

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
                    _console.WriteLine(AppConstants.Messages.InvalidDateRange);
                }
                else
                {
                    break;
                }
            }
            while (true);

            if (categoryId is null && fromDate is null && toDate is null)
            {
                return null;
            }

            return new ExpenseListFilter(categoryId, fromDate, toDate);
        }

        private async Task DeleteExpense()
        {
            var expenseId = Input.RequiredInt(AppConstants.Prompts.ExpenseIdToDelete, AppConstants.Values.MinExpenseId, int.MaxValue);
            await deleteExpense.ExecuteAsync(expenseId, CancellationToken.None);
            _console.WriteLine(AppConstants.Messages.ExpenseDeleted);
        }
    }
}
