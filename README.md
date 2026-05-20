# Personal Finance Manager — Client

Console application that connects to the [Personal Finance Manager API](https://github.com/Vishwas-2003/Personal-Finance-Manager-Server) so users can track expenses, income, budgets, and view financial summaries from the terminal.

## Features

- **Auth** — Register, login, logout; session saved locally (`.pfm.session.json`)
- **Expenses** — Add, list (optional category + from/to date filters), delete
- **Income** — Add, list, delete
- **Budgets** — Add, list, delete
- **Summary** — Income summary, expense summary, **balance summary** (credit/debit + net balance with optional date range)
- **Profile** — View logged-in user details
- **HTTP** — Automatic JWT attach, refresh on `401`, friendly errors when the API is unreachable or session expires

## Tech stack

- .NET 10 console app
- `Microsoft.Extensions.Hosting` + DI
- `IHttpClientFactory` (authenticated vs unauthenticated clients)
- Delegating handler for bearer token and refresh
- JSON session file for tokens

## Solution structure

| Layer | Folder | Role |
|-------|--------|------|
| UI | `ConsoleUi/` | Menus and user input |
| Application | `Application/` | Use cases (login, list expenses, etc.) |
| Infrastructure | `Infrastructure/` | API clients, HTTP, session file, JWT helpers |

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- **Server API running** (default: `https://localhost:7195`)

## Configuration

Edit `src/WebApp.Client/appsettings.json`:

```json
{
  "Api": {
    "BaseUrl": "https://localhost:7195"
  },
  "Auth": {
    "SessionFileName": ".pfm.session.json"
  }
}
