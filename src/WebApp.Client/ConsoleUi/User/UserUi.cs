using WebApp.Client.Application.User.Interfaces;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.ConsoleUi.User.Interfaces;
using WebApp.Client.Constants;

namespace WebApp.Client.ConsoleUi.User;

public sealed class UserUi(IGetUserProfile getUserProfile) : IUserUi
{
    private readonly IConsole _console = new SystemConsole();

    public async Task RunAsync()
    {
        var profile = await getUserProfile.ExecuteAsync(CancellationToken.None);
        _console.WriteLine(string.Empty);
        _console.WriteLine(AppConstants.Titles.UserProfile);
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileIdFormat, profile.Id));
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileNameFormat, profile.Name));
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileMobileFormat, profile.MobileNumber));
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileAgeFormat, profile.Age));
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileEmailFormat, profile.Email));
        _console.WriteLine(string.Format(AppConstants.Messages.UserProfileAddressFormat, profile.Address));
    }
}
