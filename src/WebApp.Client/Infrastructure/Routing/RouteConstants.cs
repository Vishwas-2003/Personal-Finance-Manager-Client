namespace WebApp.Client.Infrastructure.Routing;

public static class RouteConstants
{
    public static class Auth
    {
        public const string Register = "/api/Auth/register";
        public const string Login = "/api/Auth/login";
        public const string Refresh = "/api/Auth/refresh";
    }

    public static class Expense
    {
        public const string Add = "/api/Expense/add";
        public const string GetByUserId = "/api/Expense/get/{userId}";
        public const string DeleteById = "/api/Expense/delete/{expenseId}";
    }

    public static class Income
    {
        public const string Add = "/api/Income/add";
        public const string GetByUserId = "/api/Income/get/{userId}";
        public const string DeleteById = "/api/Income/delete/{incomeId}";
    }
}

