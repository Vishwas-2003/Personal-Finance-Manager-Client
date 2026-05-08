using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Expense.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.ConsoleUi;

public sealed class App(
    ISessionStore sessionStore,
    ISessionAccessor sessionAccessor,
    ILogout logout,
    IAuthUi authUi,
    IExpenseUi expenseUi)
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

                if (choice == AppConstants.Values.ExpenseChoice)
                {
                    await expenseUi.RunAsync();
                }
                else if (choice == AppConstants.Values.LogoutChoice)
                {
                    await logout.ExecuteAsync(CancellationToken.None);
                    _console.WriteLine(AppConstants.Messages.LoggedOut);
                }
                else
                {
                    return;
                }
            }
            catch (ApiException ex)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.ApiErrorFormat, (int)ex.StatusCode, ex.Message));
            }
            catch (Exception ex)
            {
                _console.WriteLine(string.Format(AppConstants.Messages.ErrorFormat, ex.Message));
            }
        }
    }

    private async Task LoadSessionAsync()
    {
        var session = await sessionStore.LoadAsync(CancellationToken.None);
        sessionAccessor.Set(session);
    }
}

