using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Utilities;

namespace WebApp.Client.ConsoleUi.Summary;

public sealed class SummaryUi(
    IGetIncomeSummary getIncomeSummary,
    IGetExpenseSummary getExpenseSummary,
    IGetBalanceSummary getBalanceSummary,
    IConsoleClearCoordinator consoleClearCoordinator) : ISummaryUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        while (true)
        {
            PrintSummaryMenu();
            var choice = Input.RequiredMenuChoice(
                AppConstants.Prompts.ChooseOption,
                AppConstants.Values.MenuMinChoice,
                AppConstants.Values.SummaryMenuMaxChoice,
                ClearAndRedrawSummaryMenu);

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

    private void PrintSummaryMenu()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.SummaryManagement, ConsoleMessageKind.Title);
        _console.WriteLine(AppConstants.Menus.Summary, ConsoleMessageKind.Menu);
    }

    private void ClearAndRedrawSummaryMenu()
    {
        consoleClearCoordinator.ClearScreen();
        PrintSummaryMenu();
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
        if (Input.ShouldSkipAllFilters(AppConstants.Prompts.SkipAllFilters))
        {
            return null;
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

        if (fromDate is null && toDate is null)
        {
            return null;
        }

        return new BalanceSummaryFilter(fromDate, toDate);
    }

    private void PrintBalanceSummary(BalanceSummary summary)
    {
        _console.WriteLine(AppConstants.Messages.BalanceSummaryCreditHeader, ConsoleMessageKind.Title);
        if (summary.Credits.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBalanceCredits, ConsoleMessageKind.Muted);
        }
        else
        {
            var headers = new[] { "Id", "Amount", "Date", "Category", "Type", "Source", "Notes" };
            var rows = summary.Credits.Select(credit => new[]
            {
                credit.Id.ToString(),
                NumberFormatUtility.FormatIndian(credit.Amount),
                credit.Date.ToString(AppConstants.Formats.Date),
                credit.CategoryName,
                credit.CategoryType,
                credit.Source,
                credit.Notes ?? string.Empty
            });
            ConsoleTableUtility.Print(_console, headers, rows);
        }

        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Messages.BalanceSummaryDebitHeader, ConsoleMessageKind.Title);
        if (summary.Debits.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoBalanceDebits, ConsoleMessageKind.Muted);
        }
        else
        {
            var headers = new[] { "Id", "Amount", "Date", "Category", "Type", "Description" };
            var rows = summary.Debits.Select(debit => new[]
            {
                debit.Id.ToString(),
                NumberFormatUtility.FormatIndian(debit.Amount),
                debit.Date.ToString(AppConstants.Formats.Date),
                debit.CategoryName,
                debit.CategoryType,
                debit.Description ?? string.Empty
            });
            ConsoleTableUtility.Print(_console, headers, rows);
        }

        _console.WriteLine(string.Empty);
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryTotalCreditFormat, NumberFormatUtility.FormatIndian(summary.TotalCredit)), ConsoleMessageKind.Highlight);
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryTotalDebitFormat, NumberFormatUtility.FormatIndian(summary.TotalDebit)), ConsoleMessageKind.Highlight);
        _console.WriteLine(string.Format(AppConstants.Messages.BalanceSummaryBalanceFormat, NumberFormatUtility.FormatIndian(summary.Balance)), ConsoleMessageKind.Success);
    }

    private void PrintIncomeHierarchy(IncomeSummary summary)
    {
        if (summary.CategoryTypeSections.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoIncome, ConsoleMessageKind.Muted);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, NumberFormatUtility.FormatIndian(summary.TotalIncome)), ConsoleMessageKind.Success);
            return;
        }

        foreach (var categoryTypeSection in summary.CategoryTypeSections)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryTypeSection.CategoryTypeName), ConsoleMessageKind.Title);

            foreach (var subCategorySection in categoryTypeSection.SubCategorySections)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, subCategorySection.CategoryName), ConsoleMessageKind.Header);
                _console.WriteLine(AppConstants.Messages.SummaryIncomeDetailHeader, ConsoleMessageKind.Header);

                foreach (var incomeEntry in subCategorySection.IncomeEntries)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryIncomeDetailRowFormat,
                        incomeEntry.Id,
                        NumberFormatUtility.FormatIndian(incomeEntry.Amount),
                        incomeEntry.Date.ToString(AppConstants.Formats.Date),
                        incomeEntry.Source,
                        incomeEntry.Notes), ConsoleMessageKind.Data);
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    subCategorySection.CategoryName,
                    NumberFormatUtility.FormatIndian(subCategorySection.Subtotal)), ConsoleMessageKind.Highlight);
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryTypeSection.CategoryTypeName,
                NumberFormatUtility.FormatIndian(categoryTypeSection.SectionTotal)), ConsoleMessageKind.Highlight);
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalIncomeFormat, NumberFormatUtility.FormatIndian(summary.TotalIncome)), ConsoleMessageKind.Success);
    }

    private void PrintExpenseHierarchy(ExpenseSummary summary)
    {
        if (summary.CategoryTypeSections.Count == 0)
        {
            _console.WriteLine(AppConstants.Messages.NoExpenses, ConsoleMessageKind.Muted);
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, NumberFormatUtility.FormatIndian(summary.TotalExpense)), ConsoleMessageKind.Success);
            return;
        }

        foreach (var categoryTypeSection in summary.CategoryTypeSections)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryTypeHeader, categoryTypeSection.CategoryTypeName), ConsoleMessageKind.Title);

            foreach (var subCategorySection in categoryTypeSection.SubCategorySections)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.SummaryCategoryHeader, subCategorySection.CategoryName), ConsoleMessageKind.Header);
                _console.WriteLine(AppConstants.Messages.SummaryExpenseDetailHeader, ConsoleMessageKind.Header);

                foreach (var expenseEntry in subCategorySection.ExpenseEntries)
                {
                    _console.WriteLine(string.Format(
                        AppConstants.Messages.SummaryExpenseDetailRowFormat,
                        expenseEntry.Id,
                        NumberFormatUtility.FormatIndian(expenseEntry.Amount),
                        expenseEntry.Date.ToString(AppConstants.Formats.Date),
                        expenseEntry.Description), ConsoleMessageKind.Data);
                }

                _console.WriteLine(string.Format(
                    AppConstants.Messages.SummaryCategorySubtotalFormat,
                    subCategorySection.CategoryName,
                    NumberFormatUtility.FormatIndian(subCategorySection.Subtotal)), ConsoleMessageKind.Highlight);
                _console.WriteLine(string.Empty);
            }

            _console.WriteLine(string.Format(
                AppConstants.Messages.SummarySectionSubtotalFormat,
                categoryTypeSection.CategoryTypeName,
                NumberFormatUtility.FormatIndian(categoryTypeSection.SectionTotal)), ConsoleMessageKind.Highlight);
            _console.WriteLine(string.Empty);
        }

        _console.WriteLine(string.Format(AppConstants.Messages.SummaryGrandTotalExpenseFormat, NumberFormatUtility.FormatIndian(summary.TotalExpense)), ConsoleMessageKind.Success);
    }
}
