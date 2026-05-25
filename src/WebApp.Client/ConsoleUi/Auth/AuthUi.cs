using WebApp.Client.Application.Auth;
using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.ConsoleUi.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;

namespace WebApp.Client.ConsoleUi.Auth;

public sealed class AuthUi(
    IRegisterUser registerUser,
    ILogin login,
    IConsoleClearCoordinator consoleClearCoordinator)
    : IAuthUi
{
    private readonly IConsole _console = new SystemConsole();
    private InputReader Input => new(_console);

    public async Task<bool> RunAsync(int choice)
    {
        if (choice == AppConstants.Values.RegisterChoice)
        {
            await RegisterAsync();
        }
        else if (choice == AppConstants.Values.LoginChoice)
        {
            await LoginAsync();
        }
        else
        {
            return true;
        }

        return false;
    }

    private async Task RegisterAsync()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.Register, ConsoleMessageKind.Title);

        var name = Input.RequiredString(AppConstants.Prompts.Name);
        var mobile = Input.RequiredString(AppConstants.Prompts.MobileNumber);
        var age = Input.RequiredInt(AppConstants.Prompts.Age, AppConstants.Values.MinAge, AppConstants.Values.MaxAge);
        var address = Input.RequiredString(AppConstants.Prompts.Address);
        var email = Input.RequiredString(AppConstants.Prompts.Email);
        var password = Input.RequiredPassword(AppConstants.Prompts.Password);

        var input = new RegisterInput(name, mobile, age, address, email, password);
        await registerUser.ExecuteAsync(input, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.RegisteredAndLoggedIn, ConsoleMessageKind.Success);
        await consoleClearCoordinator.ClearAfterLoginAsync(CancellationToken.None);
    }

    private async Task LoginAsync()
    {
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.Login, ConsoleMessageKind.Title);

        var email = Input.RequiredString(AppConstants.Prompts.Email);
        var password = Input.RequiredPassword(AppConstants.Prompts.Password);
        await login.ExecuteAsync(email, password, CancellationToken.None);
        _console.WriteLine(AppConstants.Messages.LoggedIn, ConsoleMessageKind.Success);
        await consoleClearCoordinator.ClearAfterLoginAsync(CancellationToken.None);
    }
}
