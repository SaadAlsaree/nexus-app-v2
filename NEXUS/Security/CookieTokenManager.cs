using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NEXUS.Security;

public interface ICookieTokenManager
{
    void SetTokenCookie(HttpContext context, string token, bool isPersistent = false);
    string? GetTokenCookie(HttpContext context);
    void ClearTokenCookie(HttpContext context);
}

public class CookieTokenManager : ICookieTokenManager
{
    private const string TokenCookieName = "auth_token";

    public void SetTokenCookie(HttpContext context, string token, bool isPersistent = false)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = isPersistent
                ? DateTimeOffset.UtcNow.AddDays(30)
                : DateTimeOffset.UtcNow.AddMinutes(30),
            Path = "/"
        };

        context.Response.Cookies.Append(TokenCookieName, token, cookieOptions);
    }

    public string? GetTokenCookie(HttpContext context)
    {
        return context.Request.Cookies[TokenCookieName];
    }

    public void ClearTokenCookie(HttpContext context)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            Path = "/"
        };

        context.Response.Cookies.Append(TokenCookieName, "", cookieOptions);
    }
}
