namespace AmeenAuth.Models;

/// <summary>
/// Internal representation of a user for authentication (mock/in-memory store).
/// In a database-backed app, this would map to an EF entity.
/// </summary>
public class UserAccount
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    /// <summary>ASP.NET Core Identity-compatible password hash (includes salt).</summary>
    public string PasswordHash { get; init; } = string.Empty;

    public string? DisplayName { get; init; }
}
