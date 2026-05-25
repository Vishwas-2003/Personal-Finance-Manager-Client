using WebApp.Client.ConsoleUi.Interfaces;

namespace WebApp.Client.ConsoleUi;

public sealed class SystemConsole : IConsole
{
    public void Write(string value) => Console.Write(value);

    public void WriteLine(string value) => WriteLine(value, ConsoleMessageKind.Default);

    public void WriteLine(string value, ConsoleMessageKind kind)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = kind switch
        {
            ConsoleMessageKind.Success => ConsoleColor.Green,
            ConsoleMessageKind.Warning => ConsoleColor.Yellow,
            ConsoleMessageKind.Error => ConsoleColor.Red,
            ConsoleMessageKind.Info => ConsoleColor.Cyan,
            ConsoleMessageKind.Title => ConsoleColor.Magenta,
            ConsoleMessageKind.Menu => ConsoleColor.DarkCyan,
            ConsoleMessageKind.Header => ConsoleColor.Blue,
            ConsoleMessageKind.Data => ConsoleColor.Gray,
            ConsoleMessageKind.Highlight => ConsoleColor.DarkGreen,
            ConsoleMessageKind.Muted => ConsoleColor.DarkGray,
            ConsoleMessageKind.Archived => ConsoleColor.DarkYellow,
            _ => ConsoleColor.White
        };

        Console.WriteLine(value);
        Console.ForegroundColor = previousColor;
    }

    public string? ReadLine() => Console.ReadLine();

    public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

    public void Clear() => Console.Clear();
}
