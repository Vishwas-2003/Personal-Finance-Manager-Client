using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Income;
using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.Constants;

namespace WebApp.Client.ConsoleUi.Summary;

public sealed class SummaryUi(
    IGetIncomeSummary getIncomeSummary,
    IGetExpenseSummary getExpenseSummary) : ISummaryUi
{
    private readonly IConsole _console = new SystemConsole();

    public async Task RunAsync()
    {
        while (true)
        {
            _console.WriteLine(string.Empty);
            _console.WriteLine(AppConstants.Titles.SummaryManagement);
            _console.WriteLine(AppConstants.Menus.Summary);
            var choice = new InputReader(_console).RequiredInt(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.SummaryMenuMaxChoice);

            if (choice == AppConstants.Values.MenuMinChoice)
            {
                return;
            }

            if (choice == AppConstants.Values.IncomeSummaryChoice)
            {
                await ShowIncomeSummaryAsync();
            }
            else if (choice == AppConstants.Values.ExpenseSummaryChoice)
            {
                await ShowExpenseSummaryAsync();
            }
        }
    }

    private async Task ShowIncomeSummaryAsync()
    {
        var summary = await getIncomeSummary.ExecuteAsync(CancellationToken.None);
        PrintIncomeHierarchy(summary);
    }

    private async Task ShowExpenseSummaryAsync()
    {
        var summary = await getExpenseSummary.ExecuteAsync(CancellationToken.None);
        PrintExpenseHierarchy(summary);
    }

    private void PrintIncomeHierarchy(IncomeSummary summary)
    {
        if (summary.CategoryTypes.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoIncome);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, summary.TotalIncome));
            return;
        }

        foreach (var categoryType in summary.CategoryTypes)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryType.CategoryType));

            foreach (var category in categoryType.Categories)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, category.CategoryName));
                _console.WriteLine(AppConstants.Messages.SummaryIncomeDetailHeader);

                foreach (var income in category.Items)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryIncomeDetailRowFormat,
                        income.Id,
                        income.Amount,
                        income.Date.ToString(AppConstants.Formats.Date),
                        income.Source,
                        income.Notes));
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    category.CategoryName,
                    category.Subtotal));
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryType.CategoryType,
                categoryType.Subtotal));
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, summary.TotalIncome));
    }

    private void PrintExpenseHierarchy(ExpenseSummary summary)
    {
        if (summary.CategoryTypes.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoExpenses);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, summary.TotalExpense));
            return;
        }

        foreach (var categoryType in summary.CategoryTypes)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryType.CategoryType));

            foreach (var category in categoryType.Categories)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, category.CategoryName));
                _console.WriteLine(AppConstants.Messages.SummaryExpenseDetailHeader);

                foreach (var expense in category.Items)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryExpenseDetailRowFormat,
                        expense.Id,
                        expense.Amount,
                        expense.Date.ToString(AppConstants.Formats.Date),
                        expense.Description));
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    category.CategoryName,
                    category.Subtotal));
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryType.CategoryType,
                categoryType.Subtotal));
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, summary.TotalExpense));
    }
}
