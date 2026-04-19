using AmeenAuth.Models;
using AmeenAuth.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AmeenAuth.Services;

/// <summary>
/// Development-friendly user store: loads users from configuration and hashes passwords once at startup.
/// </summary>
public sealed class InMemoryUserStore : IUserStore
{
    private readonly Dictionary<string, UserAccount> _users;

    public InMemoryUserStore(IOptions<AuthSeedOptions> options, IPasswordHasher<UserAccount> passwordHasher)
    {
        var seed = options.Value;
        _users = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase);

        foreach (var seedUser in seed.Users)
        {
            if (string.IsNullOrWhiteSpace(seedUser.Email) || string.IsNullOrEmpty(seedUser.Password))
                continue;

            var email = seedUser.Email.Trim();
            var id = Guid.NewGuid();
            var placeholder = new UserAccount
            {
                Id = id,
                Email = email,
                DisplayName = seedUser.DisplayName,
                PasswordHash = string.Empty
            };

            var hash = passwordHasher.HashPassword(placeholder, seedUser.Password);
            var account = new UserAccount
            {
                Id = id,
                Email = email,
                DisplayName = seedUser.DisplayName,
                PasswordHash = hash
            };

            _users[email] = account;
        }
    }

    public Task<UserAccount?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Task.FromResult<UserAccount?>(null);

        _users.TryGetValue(email.Trim(), out var user);
        return Task.FromResult(user);
    }
}
