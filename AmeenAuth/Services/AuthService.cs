using System.Security.Claims;
using AmeenAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace AmeenAuth.Services;

/// <summary>
/// Cookie sign-in using ASP.NET Core Identity's password hasher (PBKDF2).
/// </summary>
public sealed class AuthService : IAuthService
{
    private readonly IUserStore _userStore;
    private readonly IPasswordHasher<UserAccount> _passwordHasher;

    public AuthService(IUserStore userStore, IPasswordHasher<UserAccount> passwordHasher)
    {
        _userStore = userStore;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> SignInAsync(
        HttpContext httpContext,
        string email,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        var user = await _userStore.FindByEmailAsync(email, cancellationToken).ConfigureAwait(false);
        if (user is null)
            return AuthResult.Failed("Invalid email or password.");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verify == PasswordVerificationResult.Failed)
            return AuthResult.Failed("Invalid email or password.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.DisplayName ?? user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // "Remember me" extends cookie lifetime; otherwise use a shorter session-style expiry.
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            AllowRefresh = true,
            ExpiresUtc = rememberMe
                ? DateTimeOffset.UtcNow.AddDays(14)
                : DateTimeOffset.UtcNow.AddHours(12)
        };

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties).ConfigureAwait(false);

        return AuthResult.Success();
    }
}
