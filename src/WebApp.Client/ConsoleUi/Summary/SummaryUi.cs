using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.ConsoleUi;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Summary;

public sealed class SummaryUi(
    IGetIncomeSummary getIncomeSummary,
    IGetExpenseSummary getExpenseSummary,
    IGetBalanceSummary getBalanceSummary) : ISummaryUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            _console.WriteLine(string.Empty);
            _console.WriteLine(AppConstants.Titles.SummaryManagement);
            _console.WriteLine(AppConstants.Menus.Summary);
            var choice = Input.RequiredInt(
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
            else if (choice == AppConstants.Values.BalanceSummaryChoice)
            {
                await ShowBalanceSummaryAsync();
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

    private async Task ShowBalanceSummaryAsync()
    {
        var filter = ReadBalanceFilter();
        var summary = await getBalanceSummary.ExecuteAsync(filter, CancellationToken.None);
        PrintBalanceSummary(summary);
    }

    private BalanceSummaryFilter? ReadBalanceFilter()
    {
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

        if (fromDate is null && toDate is null)
        {
            return null;
        }

        return new BalanceSummaryFilter(fromDate, toDate);
    }

    private void PrintBalanceSummary(BalanceSummary summary)
    {
        _console.WriteLine(AppConstants.Messages.BalanceSummaryCreditHeader);
        if (summary.Credits.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBalanceCredits);
        }
        else
        {
            _console.WriteLine(AppConstants.Messages.BalanceSummaryCreditDetailHeader);
            foreach (var credit in summary.Credits)
            {
                _console.WriteLine(string.Format(
                    AppConstants.Messages.BalanceSummaryCreditRowFormat,
                    credit.Id,
                    NumberFormatUtility.FormatIndian(credit.Amount),
                    credit.Date.ToString(AppConstants.Formats.Date),
                    credit.CategoryName,
                    credit.CategoryType,
                    credit.Source,
                    credit.Notes));
            }
        }

        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Messages.BalanceSummaryDebitHeader);
        if (summary.Debits.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBalanceDebits);
        }
        else
        {
            _console.WriteLine(AppConstants.Messages.BalanceSummaryDebitDetailHeader);
            foreach (var debit in summary.Debits)
            {
                _console.WriteLine(string.Format(
                    AppConstants.Messages.BalanceSummaryDebitRowFormat,
                    debit.Id,
                    NumberFormatUtility.FormatIndian(debit.Amount),
                    debit.Date.ToString(AppConstants.Formats.Date),
                    debit.CategoryName,
                    debit.CategoryType,
                    debit.Description));
            }
        }

        _console.WriteLine(string.Empty);
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryTotalCreditFormat, NumberFormatUtility.FormatIndian(summary.TotalCredit)));
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryTotalDebitFormat, NumberFormatUtility.FormatIndian(summary.TotalDebit)));
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryBalanceFormat, NumberFormatUtility.FormatIndian(summary.Balance)));
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
