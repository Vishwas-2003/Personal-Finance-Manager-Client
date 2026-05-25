namespace WebApp.Client.ConsoleUi.Interfaces;

public interface IConsole
{
    void Write(string value);
    void WriteLine(string value);
    void WriteLine(string value, ConsoleMessageKind kind);
    string? ReadLine();
    ConsoleKeyInfo ReadKey(bool intercept);
    void Clear();
}
