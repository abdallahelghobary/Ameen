namespace AmeenAuth.Options;

/// <summary>
/// Binds to configuration section <c>Auth:Seed</c>. Used to seed in-memory users in Development.
/// Passwords are hashed at startup — keep plaintext passwords out of production configuration.
/// </summary>
public class AuthSeedOptions
{
    public const string SectionName = "Auth:Seed";

    public List<SeedUser> Users { get; set; } = new();
}

public class SeedUser
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? DisplayName { get; set; }
}
