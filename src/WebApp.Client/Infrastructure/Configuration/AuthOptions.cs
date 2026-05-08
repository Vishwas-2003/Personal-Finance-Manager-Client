using System.ComponentModel.DataAnnotations;
using WebApp.Client.Constants;

namespace WebApp.Client.Infrastructure.Configuration;

public sealed class AuthOptions
{
    [Required]
    [MinLength(1)]
    public string SessionFileName { get; init; } = AppConstants.Configuration.SessionFileName;
}

