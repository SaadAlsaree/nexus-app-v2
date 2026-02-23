using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NEXUS.Data;
using NEXUS.Security;

namespace NEXUS.Identity.Services;

public interface IIdentityService
{
    Task<bool> LoginAsync(string email, string password);
    Task<IResult> RegisterAsync(RegisterRequest request);
    Task<IResult> LogoutAsync(HttpContext context);
    Task<(bool Success, string Message)> CheckPasswordAsync(ApplicationUser user, string password);
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> FindByIdAsync(string userId);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
}

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICookieTokenManager _cookieTokenManager;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ICookieTokenManager cookieTokenManager,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cookieTokenManager = cookieTokenManager;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !user.IsActive)
        {
            _logger.LogWarning("Login failed for non-existent or inactive user: {Email}", email);
            return false;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email!,
                roles.FirstOrDefault() ?? "User",
                roles,
                DateTime.UtcNow.AddMinutes(30));

            var context = HttpContext.Current;
            if (context != null)
            {
                _cookieTokenManager.SetTokenCookie(context, accessToken);
            }

            _logger.LogInformation("User logged in successfully: {Email}", email);
        }
        else
        {
            _logger.LogWarning("Login failed for user: {Email}, Reason: {Reason}",
                email,
                result.IsLockedOut ? "Account locked" :
                result.IsNotAllowed ? "Account not allowed" :
                result.RequiresTwoFactor ? "Two factor required" :
                result.IsLockedOut ? "Invalid credentials" : "Unknown");
        }

        return result.Succeeded;
    }

    public async Task<IResult> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            Department = request.Department,
            JobTitle = request.JobTitle,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Registration failed: {Email}, Errors: {Errors}",
                request.Email, errors);

            return Results.BadRequest(new { Errors = errors });
        }

        await _userManager.AddToRoleAsync(user, "User");

        _logger.LogInformation("User registered successfully: {Email}", request.Email);

        return Results.Ok(new
        {
            Message = "Registration successful. Please login.",
            User = new
            {
                Email = user.Email,
                FullName = user.FullName,
                Department = user.Department,
                JobTitle = user.JobTitle
            }
        });
    }

    public async Task<IResult> LogoutAsync(HttpContext context)
    {
        var userId = _cookieTokenManager.GetTokenCookie(context);
        if (userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                _logger.LogInformation("User logged out: {Email}", user.Email);
            }
        }

        _cookieTokenManager.ClearTokenCookie(context);

        return Results.Ok(new { Message = "Logged out successfully" });
    }

    public async Task<(bool Success, string Message)> CheckPasswordAsync(ApplicationUser user, string password)
    {
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded
            ? (true, "Password validated")
            : (false, "Invalid password");
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}

public static class HttpContextExtensions
{
    public static HttpContext? Current { get; private set; }

    public static void SetCurrent(this HttpContext httpContext)
    {
        Current = httpContext;
    }
}
