using AmeenAuth.Models;

namespace AmeenAuth.Services;

/// <summary>
/// Abstraction over user lookup so the app can swap in EF Core or another provider later.
/// </summary>
public interface IUserStore
{
    Task<UserAccount?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
}
