using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Dto.User;

public class RegisterUserDto
{
	[Required]
	[StringLength(50, MinimumLength = 3)]
	public string Username { get; set; }

	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }

	[Required]
	[StringLength(100, MinimumLength = 6)]
	public string Password { get; set; }

	[Required]
	[Compare("Password", ErrorMessage = "Passwords do not match.")]
	public string PasswordConfirm { get; set; }
}