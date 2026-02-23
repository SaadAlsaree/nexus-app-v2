using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NEXUS.Identity.Services;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IIdentityService identityService, ILogger<AuthController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<Results<Ok<object>, UnauthorizedHttpResult>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _identityService.LoginAsync(request.Email, request.Password);

            if (!result)
            {
                return Results.Unauthorized();
            }

            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var roles = await _identityService.GetUserRolesAsync(user);
            var accessToken = Request.Cookies["auth_token"] ?? string.Empty;

            return Results.Ok(new
            {
                Success = true,
                Message = "Login successful",
                User = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Department = user.Department,
                    JobTitle = user.JobTitle,
                    Roles = roles
                },
                Token = accessToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
            return Results.StatusCode(500);
        }
    }

    [HttpPost("register")]
    public async Task<Results<Ok<object>, BadRequestObjectResult>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            return await _identityService.RegisterAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration error");
            return Results.BadRequest(new { Error = "Registration failed" });
        }
    }

    [HttpPost("logout")]
    public async Task<Results<Ok<object>, StatusCodeResult>> Logout()
    {
        try
        {
            return await _identityService.LogoutAsync(HttpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logout error");
            return Results.StatusCode(500);
        }
    }

    [HttpGet("me")]
    public async Task<Results<Ok<object>, UnauthorizedHttpResult>> GetCurrentUserInfo()
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var user = await _identityService.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var roles = await _identityService.GetUserRolesAsync(user);

            return Results.Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Department = user.Department,
                JobTitle = user.JobTitle,
                IsActive = user.IsActive,
                LastLogin = user.LastLogin,
                Roles = roles
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user info");
            return Results.StatusCode(500);
        }
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
}
