using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Infrastructure.Session;

public sealed class FileSessionStore(IOptions<AuthOptions> authOptionsAccessor) : ISessionStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<SessionModel?> LoadAsync(CancellationToken cancellationToken)
    {
        var path = GetPath();
        if (!File.Exists(path))
        {
            return null;
        }

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<SessionModel>(stream, JsonOptions, cancellationToken);
    }

    public async Task SaveAsync(SessionModel session, CancellationToken cancellationToken)
    {
        var path = GetPath();
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, session, JsonOptions, cancellationToken);
    }

    public Task ClearAsync(CancellationToken cancellationToken)
    {
        var path = GetPath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }

    private string GetPath()
    {
        var fileName = authOptionsAccessor.Value.SessionFileName;
        return Path.Combine(AppContext.BaseDirectory, fileName);
    }
}

