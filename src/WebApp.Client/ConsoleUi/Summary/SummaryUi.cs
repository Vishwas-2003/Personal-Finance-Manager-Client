using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

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
        if (summary.CategoryTypeSections.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoIncome);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, NumberFormatUtility.FormatIndian(summary.TotalIncome)));
            return;
        }

        foreach (var categoryTypeSection in summary.CategoryTypeSections)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryTypeSection.CategoryTypeName));

            foreach (var subCategorySection in categoryTypeSection.SubCategorySections)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, subCategorySection.CategoryName));
                _console.WriteLine(AppConstants.Messages.SummaryIncomeDetailHeader);

                foreach (var incomeEntry in subCategorySection.IncomeEntries)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryIncomeDetailRowFormat,
                        incomeEntry.Id,
                        NumberFormatUtility.FormatIndian(incomeEntry.Amount),
                        incomeEntry.Date.ToString(AppConstants.Formats.Date),
                        incomeEntry.Source,
                        incomeEntry.Notes));
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    subCategorySection.CategoryName,
                    NumberFormatUtility.FormatIndian(subCategorySection.Subtotal)));
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryTypeSection.CategoryTypeName,
                NumberFormatUtility.FormatIndian(categoryTypeSection.SectionTotal)));
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, NumberFormatUtility.FormatIndian(summary.TotalIncome)));
    }

    private void PrintExpenseHierarchy(ExpenseSummary summary)
    {
        if (summary.CategoryTypeSections.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoExpenses);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, NumberFormatUtility.FormatIndian(summary.TotalExpense)));
            return;
        }

        foreach (var categoryTypeSection in summary.CategoryTypeSections)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryTypeSection.CategoryTypeName));

            foreach (var subCategorySection in categoryTypeSection.SubCategorySections)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, subCategorySection.CategoryName));
                _console.WriteLine(AppConstants.Messages.SummaryExpenseDetailHeader);

                foreach (var expenseEntry in subCategorySection.ExpenseEntries)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryExpenseDetailRowFormat,
                        expenseEntry.Id,
                        NumberFormatUtility.FormatIndian(expenseEntry.Amount),
                        expenseEntry.Date.ToString(AppConstants.Formats.Date),
                        expenseEntry.Description));
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    subCategorySection.CategoryName,
                    NumberFormatUtility.FormatIndian(subCategorySection.Subtotal)));
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryTypeSection.CategoryTypeName,
                NumberFormatUtility.FormatIndian(categoryTypeSection.SectionTotal)));
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, NumberFormatUtility.FormatIndian(summary.TotalExpense)));
    }
}
