using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApp.Client.Application.Auth;
using WebApp.Client.Application.Auth.Interfaces;
using WebApp.Client.Application.Budget;
using WebApp.Client.Application.Budget.Interfaces;
using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Application.Expenses;
using WebApp.Client.Application.Expenses.Interfaces;
using WebApp.Client.Application.Income;
using WebApp.Client.Application.Income.Interfaces;
using WebApp.Client.Application.Summary;
using WebApp.Client.Application.Summary.Interfaces;
using WebApp.Client.Application.User;
using WebApp.Client.Application.User.Interfaces;
using WebApp.Client.ConsoleUi;
using WebApp.Client.ConsoleUi.Auth;
using WebApp.Client.ConsoleUi.Auth.Interfaces;
using WebApp.Client.ConsoleUi.Budget;
using WebApp.Client.ConsoleUi.Budget.Interfaces;
using WebApp.Client.ConsoleUi.Expense;
using WebApp.Client.ConsoleUi.Expense.Interfaces;
using WebApp.Client.ConsoleUi.Income;
using WebApp.Client.ConsoleUi.Income.Interfaces;
using WebApp.Client.ConsoleUi.Summary;
using WebApp.Client.ConsoleUi.Summary.Interfaces;
using WebApp.Client.ConsoleUi.User;
using WebApp.Client.ConsoleUi.User.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.ApiClients.Auth;
using WebApp.Client.Infrastructure.ApiClients.Budget;
using WebApp.Client.Infrastructure.ApiClients.Category;
using WebApp.Client.Infrastructure.ApiClients.Expenses;
using WebApp.Client.Infrastructure.ApiClients.Income;
using WebApp.Client.Infrastructure.ApiClients.Summary;
using WebApp.Client.Infrastructure.ApiClients.User;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Session;
using WebApp.Client.Infrastructure.Session.Interfaces;
using WebApp.Client.Infrastructure.Token;
using WebApp.Client.Infrastructure.Token.Interfaces;

namespace WebApp.Client.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersonalFinanceManagerClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ApiOptions>()
            .Bind(configuration.GetSection(AppConstants.Configuration.ApiSection))
            .ValidateDataAnnotations();

        services.AddOptions<AuthOptions>()
            .Bind(configuration.GetSection(AppConstants.Configuration.AuthSection))
            .ValidateDataAnnotations();

        services.AddSingleton<ISessionStore, FileSessionStore>();
        services.AddSingleton<ISessionAccessor, SessionAccessor>();
        services.AddSingleton<IJwtUserIdReader, JwtUserIdReader>();

        services.AddHttpClient(ApiHttpClientNames.Unauthenticated, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddTransient<BearerTokenHandler>();
        services.AddHttpClient(ApiHttpClientNames.Authenticated, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        })
        .AddHttpMessageHandler<BearerTokenHandler>();

        services.AddSingleton<IAuthApi, AuthApi>();
        services.AddSingleton<IExpenseApi, ExpenseApi>();
        services.AddSingleton<IIncomeApi, IncomeApi>();
        services.AddSingleton<IBudgetApi, BudgetApi>();
        services.AddSingleton<ICategoryApi, CategoryApi>();
        services.AddSingleton<ISummaryApi, SummaryApi>();
        services.AddSingleton<IUserApi, UserApi>();

        services.AddSingleton<IAuthUi, AuthUi>();
        services.AddSingleton<IExpenseUi, ExpenseUi>();
        services.AddSingleton<IIncomeUi, IncomeUi>();
        services.AddSingleton<IBudgetUi, BudgetUi>();
        services.AddSingleton<ISummaryUi, SummaryUi>();
        services.AddSingleton<IUserUi, UserUi>();
        services.AddSingleton<IRegisterUser, RegisterUser>();
        services.AddSingleton<ILogin, Login>();
        services.AddSingleton<ILogout, Logout>();
        services.AddSingleton<IAddExpense, AddExpense>();
        services.AddSingleton<IListExpenses, ListExpenses>();
        services.AddSingleton<IDeleteExpense, DeleteExpense>();
        services.AddSingleton<IAddIncome, AddIncome>();
        services.AddSingleton<IListIncome, ListIncome>();
        services.AddSingleton<IDeleteIncome, DeleteIncome>();
        services.AddSingleton<IAddBudget, AddBudget>();
        services.AddSingleton<IListBudget, ListBudget>();
        services.AddSingleton<IDeleteBudget, DeleteBudget>();
        services.AddSingleton<IListCategories, ListCategory>();
        services.AddSingleton<IGetIncomeSummary, GetIncomeSummary>();
        services.AddSingleton<IGetExpenseSummary, GetExpenseSummary>();
        services.AddSingleton<IGetBalanceSummary, GetBalanceSummary>();
        services.AddSingleton<IGetUserProfile, GetUserProfile>();

        services.AddSingleton<App>();
        return services;
    }
}

