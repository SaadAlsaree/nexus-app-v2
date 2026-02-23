using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NEXUS.Security;

public interface IJwtTokenValidator
{
    ClaimsPrincipal? ValidateToken(string token);
    bool IsTokenExpired(ClaimsPrincipal principal);
    string? GetUserId(ClaimsPrincipal principal);
}

public class JwtTokenValidator : IJwtTokenValidator
{
    private readonly IConfiguration _configuration;

    public JwtTokenValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public bool IsTokenExpired(ClaimsPrincipal principal)
    {
        var expiryClaim = principal.FindFirst(JwtRegisteredClaimNames.Expires);
        if (expiryClaim == null)
            return true;

        return long.Parse(expiryClaim.Value) < DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public string? GetUserId(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
