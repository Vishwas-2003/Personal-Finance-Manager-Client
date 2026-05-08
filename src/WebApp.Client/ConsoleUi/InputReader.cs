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

            console.WriteLine(AppConstants.Messages.RequiredValue);
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

            console.WriteLine(string.Format(AppConstants.Messages.NumberBetweenFormat, minInclusive, maxInclusive));
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

            console.WriteLine(string.Format(AppConstants.Messages.NumberAtLeastFormat, minInclusive));
        }
    }

    public DateTime RequiredDate(string prompt)
    {
        while (true)
        {
            console.Write(prompt);
            var raw = console.ReadLine()?.Trim();
            if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var value))
            {
                return value;
            }

            console.WriteLine(AppConstants.Messages.InvalidDate);
        }
    }

    public string? OptionalString(string prompt)
    {
        console.Write(prompt);
        var value = console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}

