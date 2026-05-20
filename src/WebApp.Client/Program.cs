using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.ConsoleUi;
using WebApp.Client.Infrastructure.DependencyInjection;
using WebApp.Client.Infrastructure.Http;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile(AppConstants.Configuration.AppSettingsFile, optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables(prefix: AppConstants.Configuration.EnvironmentPrefix);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddPersonalFinanceManagerClient(context.Configuration);
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.None);
    })
    .Build();

var app = host.Services.GetRequiredService<App>();
var logout = host.Services.GetRequiredService<ILogout>();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    var sessionExpired = ExceptionHelper.FindSessionExpired(ex);
    if (sessionExpired is not null)
    {
        await logout.ExecuteAsync(CancellationToken.None);
        Console.WriteLine(sessionExpired.Message);
    }
    else
    {
        Console.WriteLine(AppConstants.Messages.SomethingWentWrong);
    }
}
