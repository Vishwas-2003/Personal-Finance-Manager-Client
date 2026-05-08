using WebApp.Client.ConsoleUi.Interfaces;

namespace WebApp.Client.ConsoleUi;

public sealed class SystemConsole : IConsole
{
    public void Write(string value) => Console.Write(value);
    public void WriteLine(string value) => Console.WriteLine(value);
    public string? ReadLine() => Console.ReadLine();
}

