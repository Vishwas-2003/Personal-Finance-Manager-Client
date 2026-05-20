using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Budget.Interfaces;
using WebApp.Client.ConsoleUi.Expense.Interfaces;
using WebApp.Client.ConsoleUi.Income.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.ConsoleUi.User.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.ConsoleUi;

public sealed class App(
    ISessionStore sessionStore,
    ISessionAccessor sessionAccessor,
    ILogout logout,
    IAuthUi authUi,
    IExpenseUi expenseUi,
    IIncomeUi incomeUi,
    IBudgetUi budgetUi,
    ISummaryUi summaryUi,
    IUserUi userUi)
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task RunAsync()
    {
        await LoadSessionAsync();

        _console.WriteLine(AppConstants.Titles.AppName);
        _console.WriteLine(AppConstants.Titles.AppDivider);

        while (true)
        {
            var isLoggedIn = sessionAccessor.Current is not null;
            _console.WriteLine(string.Empty);
            _console.WriteLine(isLoggedIn ? AppConstants.Menus.MainLoggedIn : AppConstants.Menus.MainLoggedOut);
            var choice = Input.RequiredInt(AppConstants.Prompts.ChooseOption, AppConstants.Values.MenuMinChoice, AppConstants.Values.MainMenuMaxChoice);

            try
            {
                if (!isLoggedIn)
                {
                    var exit = await authUi.RunAsync(choice);
                    if (exit)
                    {
                        return;
                    }
                    continue;
                }

                switch (choice)
                {
                    case AppConstants.Values.ExpenseChoice:
                        await expenseUi.RunAsync();
                        break;
                    case AppConstants.Values.IncomeChoice:
                        await incomeUi.RunAsync();
                        break;
                    case AppConstants.Values.BudgetChoice:
                        await budgetUi.RunAsync();
                        break;
                    case AppConstants.Values.SummaryChoice:
                        await summaryUi.RunAsync();
                        break;
                    case AppConstants.Values.ProfileChoice:
                        await userUi.RunAsync();
                        break;
                    case AppConstants.Values.LogoutChoice:
                        await logout.ExecuteAsync(CancellationToken.None);
                        _console.WriteLine(AppConstants.Messages.LoggedOut);
                        break;
                    default: return;
                }
            }
            catch (Exception ex)
            {
                if (!await HandleExceptionAsync(ex))
                {
                    throw;
                }
            }
        }
    }

    private async Task<bool> HandleExceptionAsync(Exception ex)
    {
        var sessionExpired = ExceptionHelper.FindSessionExpired(ex);
        if (sessionExpired is not null)
        {
            await logout.ExecuteAsync(CancellationToken.None);
            _console.WriteLine(sessionExpired.Message);
            return true;
        }

        if (ExceptionHelper.IsConnectionError(ex))
        {
            _console.WriteLine(AppConstants.Messages.SomethingWentWrong);
            return true;
        }

        if (ex is ApiException apiEx)
        {
            _console.WriteLine(string.Format(AppConstants.Messages.ApiErrorFormat, (int)apiEx.StatusCode, apiEx.Message));
            return true;
        }

        _console.WriteLine(AppConstants.Messages.SomethingWentWrong);
        return true;
    }

    private async Task LoadSessionAsync()
    {
        var session = await sessionStore.LoadAsync(CancellationToken.None);
        sessionAccessor.Set(session);
    }
}
