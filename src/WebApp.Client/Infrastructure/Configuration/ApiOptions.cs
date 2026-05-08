using System.ComponentModel.DataAnnotations;

namespace WebApp.Client.Infrastructure.Configuration;

public sealed class ApiOptions
{
    [Required]
    [Url]
    public string BaseUrl { get; init; } = string.Empty;
}

