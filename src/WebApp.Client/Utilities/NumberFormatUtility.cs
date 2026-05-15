using System.Globalization;

namespace WebApp.Client.Utilities;

/// <summary>
/// Formats numbers using the Indian numbering system (e.g. 1,00,000 and 1,00,00,000).
/// </summary>
public static class NumberFormatUtility
{
    public static string FormatIndian(decimal value, int decimalPlaces = 2)
    {
        if (decimalPlaces < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
        }

        var isNegative = value < 0;
        value = Math.Abs(value);

        string integerPart;
        string? fractionalPart = null;

        if (decimalPlaces == 0)
        {
            integerPart = decimal.Truncate(value).ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            var rounded = Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
            var formatted = rounded.ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
            var dotIndex = formatted.IndexOf('.');
            if (dotIndex < 0)
            {
                integerPart = formatted;
            }
            else
            {
                integerPart = formatted[..dotIndex];
                fractionalPart = formatted[(dotIndex + 1)..];
            }
        }

        var result = FormatIntegerPart(integerPart);
        if (fractionalPart is not null)
        {
            result += "." + fractionalPart;
        }

        return isNegative ? "-" + result : result;
    }

    private static string FormatIntegerPart(string digits)
    {
        if (string.IsNullOrWhiteSpace(digits))
        {
            return "0";
        }

        digits = digits.TrimStart('0');
        if (digits.Length == 0)
        {
            return "0";
        }

        if (digits.Length <= 3)
        {
            return digits;
        }

        var groups = new List<string> { digits[^3..] };
        var remaining = digits[..^3];

        while (remaining.Length > 0)
        {
            var groupSize = Math.Min(2, remaining.Length);
            groups.Insert(0, remaining[^groupSize..]);
            remaining = remaining[..^groupSize];
        }

        return string.Join(",", groups);
    }
}
