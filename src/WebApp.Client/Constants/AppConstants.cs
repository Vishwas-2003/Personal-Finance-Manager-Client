namespace WebApp.Client.Constants;

public static class AppConstants
{
    public static class Configuration
    {
        public const string AppSettingsFile = "appsettings.json";
        public const string EnvironmentPrefix = "PFM_";
        public const string ApiSection = "Api";
        public const string AuthSection = "Auth";
        public const string SessionFileName = ".pfm.session.json";
    }

    public static class Menus
    {
        public const string MainLoggedOut = "1) Register  2) Login  0) Exit";
        public const string MainLoggedIn = "1) Expenses  2) Income  3) Logout  0) Exit";
        public const string Expense = "1) Add expense  2) List expenses  3) Delete expense  0) Back";
        public const string Income = "1) Add income  2) List income  3) Delete income  0) Back";
    }

    public static class Titles
    {
        public const string AppName = "Personal Finance Manager (Console Client)";
        public const string AppDivider = "----------------------------------------";
        public const string Register = "== Register ==";
        public const string Login = "== Login ==";
        public const string ExpenseManagement = "== Expense Management ==";
        public const string IncomeManagement = "== Income Management ==";
    }

    public static class Prompts
    {
        public const string ChooseOption = "Choose an option: ";
        public const string Name = "Name: ";
        public const string MobileNumber = "Mobile number: ";
        public const string Age = "Age (1-120): ";
        public const string Address = "Address: ";
        public const string Email = "Email: ";
        public const string Password = "Password: ";
        public const string Amount = "Amount: ";
        public const string CategoryId = "Category id: ";
        public const string DescriptionOptional = "Description (optional): ";
        public const string Date = "Date (YYYY-MM-DD): ";
        public const string ExpenseIdToDelete = "Expense id to delete: ";
        public const string IncomeIdToDelete = "Income id to delete: ";
        public const string IncomeSource = "Income source: ";
        public const string IncomeNotesOptional = "Notes (optional): ";
    }

    public static class Messages
    {
        public const string LoggedOut = "Logged out.";
        public const string RegisteredAndLoggedIn = "Registered and logged in.";
        public const string LoggedIn = "Logged in.";
        public const string ExpenseAdded = "Expense added.";
        public const string NoExpenses = "No expenses found.";
        public const string ExpenseDeleted = "Expense deleted.";
        public const string ExpenseHeader = "Id | Amount | Category | Type | Date | Description";
        public const string ExpenseRowFormat = "{0} | {1} | {2} | {3} | {4} | {5}";
        public const string IncomeAdded = "Income added.";
        public const string NoIncome = "No income found.";
        public const string IncomeDeleted = "Income deleted.";
        public const string IncomeHeader = "Id | Amount | Category | Type | Date | Source | Notes";
        public const string IncomeRowFormat = "{0} | {1} | {2} | {3} | {4} | {5} | {6}";

        public const string ApiErrorFormat = "API error ({0}): {1}";
        public const string ErrorFormat = "Error: {0}";

        public const string RequiredValue = "Value is required.";
        public const string NumberBetweenFormat = "Enter a number between {0} and {1}.";
        public const string NumberAtLeastFormat = "Enter a number >= {0}.";
        public const string InvalidDate = "Enter a valid date (example: 2026-05-07).";

        public const string MustLoginFirst = "You must login first.";
        public const string MissingUserIdInToken = "Login succeeded but the user id could not be read from the access token.";
        public const string EmptyResponseBody = "Empty response body.";
    }

    public static class Values
    {
        public const int MenuMinChoice = 0;
        public const int MainMenuMaxChoice = 3;
        public const int ExpenseMenuMaxChoice = 3;
        public const int IncomeMenuMaxChoice = 3;
        public const int RegisterChoice = 1;
        public const int LoginChoice = 2;
        public const int ExpenseChoice = 1;
        public const int IncomeChoice = 2;
        public const int LogoutChoice = 3;
        public const int AddExpenseChoice = 1;
        public const int ListExpenseChoice = 2;
        public const int DeleteExpenseChoice = 3;
        public const int AddIncomeChoice = 1;
        public const int ListIncomeChoice = 2;
        public const int DeleteIncomeChoice = 3;
        public const int MinAge = 1;
        public const int MaxAge = 120;
        public const int MinCategoryId = 1;
        public const int MinIncomeCategoryId = 0;
        public const int MinExpenseId = 1;
        public const int MinIncomeId = 1;
        public const decimal MinAmount = 0;
    }

    public static class Http
    {
        public const string JsonMediaType = "application/json";
        public const string BearerScheme = "Bearer";
    }

    public static class Formats
    {
        public const string Date = "yyyy-MM-dd";
    }

    public static class RoutePlaceholders
    {
        public const string UserId = "{userId}";
        public const string ExpenseId = "{expenseId}";
        public const string IncomeId = "{incomeId}";
    }
}

