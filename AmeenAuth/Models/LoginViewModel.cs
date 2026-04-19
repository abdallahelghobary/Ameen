using System.ComponentModel.DataAnnotations;

namespace AmeenAuth.Models;

/// <summary>
/// Strongly-typed login form model with server-side validation via data annotations.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    /// <summary>Optional return URL after login (validated to prevent open redirects).</summary>
    public string? ReturnUrl { get; set; }
}
