namespace AmeenAuth.Services;

/// <summary>
/// Validates credentials and issues the authentication cookie (cookie authentication scheme).
/// </summary>
public interface IAuthService
{
    Task<AuthResult> SignInAsync(HttpContext httpContext, string email, string password, bool rememberMe, CancellationToken cancellationToken = default);
}

public sealed class AuthResult
{
    public bool Succeeded { get; init; }

    public string? ErrorMessage { get; init; }

    public static AuthResult Success() => new() { Succeeded = true };

    public static AuthResult Failed(string message) => new() { Succeeded = false, ErrorMessage = message };
}
