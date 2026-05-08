using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Client.Infrastructure.Token.Interfaces;

namespace WebApp.Client.Infrastructure.Token;

public sealed class JwtUserIdReader : IJwtUserIdReader
{
    public bool TryReadUserId(string jwt, out int userId)
    {
        userId = 0;
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return false;
        }

        JwtSecurityToken token;
        try
        {
            token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        }
        catch
        {
            return false;
        }

        var candidate =
            token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        return int.TryParse(candidate, out userId);
    }
}

