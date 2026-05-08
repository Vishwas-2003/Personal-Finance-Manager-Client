using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Client.Constants;
using WebApp.Client.ConsoleUi;
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
    .Build();

await host.Services.GetRequiredService<App>().RunAsync();

