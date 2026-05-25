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
        public const string MainLoggedIn = "1) Expenses  2) Income  3) Budget  4) Summary  5) Profile  6) Logout  0) Exit";
        public const string Expense = "1) Add expense  2) List expenses  3) Archive expense  4) Update expense  0) Back";
        public const string Income = "1) Add income  2) List income  3) Archive income  4) Update income  0) Back";
        public const string Budget = "1) Add budget  2) List budgets  3) Archive budget  4) Update budget  0) Back";
        public const string Summary = "1) Income summary  2) Expense summary  3) Balance summary  0) Back";
    }

    public static class Titles
    {
        public const string AppName = "Personal Finance Manager (Console Client)";
        public const string AppDivider = "----------------------------------------";
        public const string Register = "== Register ==";
        public const string Login = "== Login ==";
        public const string ExpenseManagement = "== Expense Management ==";
        public const string IncomeManagement = "== Income Management ==";
        public const string BudgetManagement = "== Budget Management ==";
        public const string SummaryManagement = "== Summary ==";
        public const string UserProfile = "== User Profile ==";
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
        public const string CategoryIdWithViewOption = "Category id: (Enter 0 to view available categories)";
        public const string CategoryId = "Category id: ";
        public const string DescriptionOptional = "Description (optional): ";
        public const string Date = "Date (YYYY-MM-DD): ";
        public const string ExpenseIdToDelete = "Expense id to archive: ";
        public const string ExpenseFilterCategoryOptional = "Filter by category id (optional, leave blank to skip, 0 to view categories): ";
        public const string ExpenseFilterFromDateOptional = "Filter from date (optional, YYYY-MM-DD, leave blank to skip): ";
        public const string ExpenseFilterToDateOptional = "Filter to date (optional, YYYY-MM-DD, leave blank to skip): ";
        public const string SkipAllFilters = "Press Enter to skip all filters, or type anything to continue: ";
        public const string KeywordFilterOptional = "Search keywords (space-separated, optional): ";
        public const string IncomeIdToDelete = "Income id to archive: ";
        public const string ExpenseIdToUpdate = "Expense id to update: ";
        public const string IncomeIdToUpdate = "Income id to update: ";
        public const string BudgetIdToUpdate = "Budget id to update: ";
        public const string IncomeSource = "Income source: ";
        public const string IncomeNotesOptional = "Notes (optional): ";
        public const string BudgetLimitAmount = "Budget limit amount (> 0): ";
        public const string BudgetSpentAmount = "Spent amount (0 if none): ";
        public const string BudgetIdToDelete = "Budget id to archive: ";
    }

    public static class Messages
    {
        public const string LoggedOut = "Logged out.";
        public const string SessionExpired = "Your session has expired. Please login again.";
        public const string RegisteredAndLoggedIn = "Registered and logged in.";
        public const string LoggedIn = "Logged in.";
        public const string ExpenseAdded = "Expense added.";
        public const string NoExpenses = "No expenses found.";
        public const string NoExpensesForFilter = "No expenses found for the selected filter(s).";
        public const string ExpenseDeleted = "Expense archived.";
        public const string ExpenseUpdated = "Expense updated.";
        public const string StatusActive = "Active";
        public const string StatusArchived = "Archived";
        public const string ExpenseHeader = "Id | Amount | Category | Type | Date | Description";
        public const string ExpenseRowFormat = "{0} | {1} | {2} | {3} | {4} | {5}";
        public const string IncomeAdded = "Income added.";
        public const string NoIncome = "No income found.";
        public const string IncomeDeleted = "Income archived.";
        public const string IncomeUpdated = "Income updated.";
        public const string IncomeHeader = "Id | Amount | Category | Type | Date | Source | Notes";
        public const string IncomeRowFormat = "{0} | {1} | {2} | {3} | {4} | {5} | {6}";
        public const string BudgetAdded = "Budget added.";
        public const string NoBudgets = "No budgets found.";
        public const string BudgetDeleted = "Budget archived.";
        public const string BudgetUpdated = "Budget updated.";
        public const string FutureDateNotAllowed = "Date cannot be in the future.";
        public const string BudgetHeader = "Id | Limit | Spent | Category | Type | Updated (UTC)";
        public const string BudgetRowFormat = "{0} | {1} | {2} | {3} | {4} | {5}";
        public const string SummaryCategoryTypeHeader = "=== {0} (category) ===";
        public const string SummaryCategoryHeader = "  -- {0} (sub-category) --";
        public const string SummaryIncomeDetailHeader = "    Id | Amount | Date | Source | Notes";
        public const string SummaryIncomeDetailRowFormat = "    {0} | {1} | {2} | {3} | {4}";
        public const string SummaryExpenseDetailHeader = "    Id | Amount | Date | Description";
        public const string SummaryExpenseDetailRowFormat = "    {0} | {1} | {2} | {3}";
        public const string SummaryCategorySubtotalFormat = "    Subtotal ({0}): {1}";
        public const string SummarySectionSubtotalFormat = "  Section total ({0}): {1}";
        public const string SummaryGrandTotalIncomeFormat = "=== Grand total income: {0} ===";
        public const string SummaryGrandTotalExpenseFormat = "=== Grand total expenses: {0} ===";
        public const string BalanceSummaryCreditHeader = "=== Credit sheet (income) ===";
        public const string BalanceSummaryCreditDetailHeader = "Id | Amount | Date | Category | Type | Source | Notes";
        public const string BalanceSummaryCreditRowFormat = "{0} | {1} | {2} | {3} | {4} | {5} | {6}";
        public const string BalanceSummaryDebitHeader = "=== Debit sheet (expenses) ===";
        public const string BalanceSummaryDebitDetailHeader = "Id | Amount | Date | Category | Type | Description";
        public const string BalanceSummaryDebitRowFormat = "{0} | {1} | {2} | {3} | {4} | {5}";
        public const string BalanceSummaryTotalCreditFormat = "Total credit: {0}";
        public const string BalanceSummaryTotalDebitFormat = "Total debit: {0}";
        public const string BalanceSummaryBalanceFormat = "Balance (credit - debit): {0}";
        public const string NoBalanceCredits = "No income entries in this period.";
        public const string NoBalanceDebits = "No expense entries in this period.";

        public const string ApiErrorFormat = "API error ({0}): {1}";
        public const string ErrorFormat = "Error: {0}";
        public const string SomethingWentWrong = "Something went wrong. Please check that the server is running and try again.";

        public const string RequiredValue = "Value is required.";
        public const string NumberBetweenFormat = "Enter a number between {0} and {1}.";
        public const string NumberAtLeastFormat = "Enter a number >= {0}.";
        public const string InvalidDate = "Enter a valid date (example: 2026-05-07).";
        public const string InvalidDateRange = "From date cannot be after to date.";

        public const string MustLoginFirst = "You must login first.";
        public const string MissingUserIdInToken = "Login succeeded but the user id could not be read from the access token.";
        public const string EmptyResponseBody = "Empty response body.";
        public const string AvailableCategoriesHeader = "Id | Name | Type";
        public const string CategoriesRowFormat = "{0} | {1} | {2}";
        public const string AvailableCategories = "Available Categories";
        public const string NoCategoriesAvailable = "No categories available.";
        public const string UserProfileIdFormat = "Id: {0}";
        public const string UserProfileNameFormat = "Name: {0}";
        public const string UserProfileMobileFormat = "Mobile: {0}";
        public const string UserProfileAgeFormat = "Age: {0}";
        public const string UserProfileEmailFormat = "Email: {0}";
        public const string UserProfileAddressFormat = "Address: {0}";
    }

    public static class Commands
    {
        public const string ClearConsole = "clear";
    }

    public static class Values
    {
        public const int ClearDelaySeconds = 2;
        public const int MenuMinChoice = 0;
        public const int MainMenuMaxChoice = 6;
        public const int ExpenseMenuMaxChoice = 4;
        public const int IncomeMenuMaxChoice = 4;
        public const int BudgetMenuMaxChoice = 4;
        public const int SummaryMenuMaxChoice = 3;
        public const int RegisterChoice = 1;
        public const int LoginChoice = 2;
        public const int ExpenseChoice = 1;
        public const int IncomeChoice = 2;
        public const int BudgetChoice = 3;
        public const int SummaryChoice = 4;
        public const int ProfileChoice = 5;
        public const int LogoutChoice = 6;
        public const int AddExpenseChoice = 1;
        public const int ListExpenseChoice = 2;
        public const int DeleteExpenseChoice = 3;
        public const int UpdateExpenseChoice = 4;
        public const int AddIncomeChoice = 1;
        public const int ListIncomeChoice = 2;
        public const int DeleteIncomeChoice = 3;
        public const int UpdateIncomeChoice = 4;
        public const int AddBudgetChoice = 1;
        public const int ListBudgetChoice = 2;
        public const int DeleteBudgetChoice = 3;
        public const int UpdateBudgetChoice = 4;
        public const int IncomeSummaryChoice = 1;
        public const int ExpenseSummaryChoice = 2;
        public const int BalanceSummaryChoice = 3;
        public const int MinAge = 1;
        public const int MaxAge = 120;
        public const int MinCategoryIdForViewOption = 0;
        public const int MinCategoryId = 1;
        public const int MinIncomeCategoryId = 0;
        public const int MinExpenseId = 1;
        public const int MinIncomeId = 1;
        public const int MinBudgetCategoryId = 0;
        public const int MinBudgetId = 1;
        public const decimal MinAmount = 0;
        public const decimal MinBudgetLimitAmount = 0.01m;
    }

    public static class Http
    {
        public const string JsonMediaType = "application/json";
        public const string BearerScheme = "Bearer";
    }

    public static class ErrorCodes
    {
        public const string SessionExpired = "SESSION_EXPIRED";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string BadRequest = "BAD_REQUEST";
        public const string NotFound = "NOT_FOUND";
        public const string Conflict = "CONFLICT";
        public const string InternalError = "INTERNAL_ERROR";
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
        public const string BudgetId = "{budgetId}";
    }
}

