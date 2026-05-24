using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Client.ConsoleUi;
using WebApp.Client.ConsoleUi.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.DependencyInjection;

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
var exceptionHandler = host.Services.GetRequiredService<IUiExceptionHandler>();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    await exceptionHandler.TryHandleAsync(ex);
}
