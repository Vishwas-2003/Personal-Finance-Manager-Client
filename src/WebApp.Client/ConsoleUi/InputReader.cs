using System.Globalization;
using WebApp.Client.Constants;
using WebApp.Client.ConsoleUi.Interfaces;

namespace WebApp.Client.ConsoleUi;

public sealed class InputReader(IConsole console)
{
    public string RequiredString(string prompt)
    {
        while (true)
        {
            console.Write(prompt);
            var value = console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            console.WriteLine(AppConstants.Messages.RequiredValue, ConsoleMessageKind.Warning);
        }
    }

    public string RequiredPassword(string prompt)
    {
        while (true)
        {
            console.Write(prompt);
            var password = ReadMaskedPassword();
            if (!string.IsNullOrWhiteSpace(password))
            {
                console.WriteLine(string.Empty);
                return password;
            }

            console.WriteLine(AppConstants.Messages.RequiredValue, ConsoleMessageKind.Warning);
        }
    }

    public int RequiredInt(string prompt, int minInclusive, int maxInclusive)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (int.TryParse(raw, out var value) && value >= minInclusive && value <= maxInclusive)
            {
                return value;
            }

            console.WriteLine(
                string.Format(AppConstants.Messages.NumberBetweenFormat, minInclusive, maxInclusive),
                ConsoleMessageKind.Warning);
        }
    }

    public int RequiredMenuChoice(string prompt, int minInclusive, int maxInclusive, Action onClearAndRedraw)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (string.Equals(raw, AppConstants.Commands.ClearConsole, StringComparison.OrdinalIgnoreCase))
            {
                onClearAndRedraw();
                continue;
            }

            if (int.TryParse(raw, out var value) && value >= minInclusive && value <= maxInclusive)
            {
                return value;
            }

            console.WriteLine(
                string.Format(AppConstants.Messages.NumberBetweenFormat, minInclusive, maxInclusive),
                ConsoleMessageKind.Warning);
        }
    }

    public decimal RequiredDecimal(string prompt, decimal minInclusive)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var value) && value >= minInclusive)
            {
                return value;
            }

            console.WriteLine(
                string.Format(AppConstants.Messages.NumberAtLeastFormat, minInclusive),
                ConsoleMessageKind.Warning);
        }
    }

    public DateTime RequiredDate(string prompt, bool disallowFuture = false)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var value))
            {
                if (disallowFuture && value.Date > DateTime.Today)
                {
                    console.WriteLine(AppConstants.Messages.FutureDateNotAllowed, ConsoleMessageKind.Warning);
                    continue;
                }

                return value;
            }

            console.WriteLine(AppConstants.Messages.InvalidDate, ConsoleMessageKind.Warning);
        }
    }

    public string? OptionalString(string prompt)
    {
        console.Write(prompt);
        var value = console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public int? OptionalInt(string prompt, int minInclusive)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            if (int.TryParse(raw, out var value) && value >= minInclusive)
            {
                return value;
            }

            console.WriteLine(
                string.Format(AppConstants.Messages.NumberAtLeastFormat, minInclusive),
                ConsoleMessageKind.Warning);
        }
    }

    public DateTime? OptionalDate(string prompt)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var value))
            {
                return value;
            }

            console.WriteLine(AppConstants.Messages.InvalidDate, ConsoleMessageKind.Warning);
        }
    }

    public bool ShouldSkipAllFilters(string prompt)
    {
        console.Write(prompt);
        var value = console.ReadLine();
        return string.IsNullOrWhiteSpace(value);
    }

    private string ReadMaskedPassword()
    {
        var password = string.Empty;
        while (true)
        {
            var key = console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[..^1];
                    console.Write("\b \b");
                }

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                console.Write("*");
            }
        }

        return password;
    }
}
