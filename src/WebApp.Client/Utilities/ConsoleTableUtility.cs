using WebApp.Client.ConsoleUi;
using WebApp.Client.ConsoleUi.Interfaces;

namespace WebApp.Client.Utilities;

public static class ConsoleTableUtility
{
    private const int MaxColumnWidth = 40;

    public static void Print(
        IConsole console,
        IReadOnlyList<string> headers,
        IEnumerable<string[]> rows,
        Func<string[], ConsoleMessageKind>? rowKindSelector = null)
    {
        var allRows = rows.Prepend(headers.ToArray()).ToList();
        var columnCount = headers.Count;
        var widths = new int[columnCount];

        for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
            widths[columnIndex] = allRows.Max(row => Truncate(row[columnIndex], MaxColumnWidth).Length);
        }

        console.WriteLine(FormatRow(headers.ToArray(), widths), ConsoleMessageKind.Header);
        console.WriteLine(new string('-', widths.Sum() + (columnCount * 3) - 1), ConsoleMessageKind.Muted);

        foreach (var row in rows)
        {
            var kind = rowKindSelector?.Invoke(row) ?? ConsoleMessageKind.Data;
            console.WriteLine(FormatRow(row, widths), kind);
        }
    }

    private static string FormatRow(string[] values, int[] widths)
    {
        var cells = new string[values.Length];
        for (var index = 0; index < values.Length; index++)
        {
            var value = Truncate(values[index] ?? string.Empty, MaxColumnWidth);
            cells[index] = value.PadRight(widths[index]);
        }

        return string.Join(" | ", cells);
    }

    private static string Truncate(string value, int maxLength) =>
        value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...";
}
