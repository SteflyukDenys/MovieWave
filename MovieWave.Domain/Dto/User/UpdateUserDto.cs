using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Dto.User;

public class UpdateUserDto
{
	[StringLength(50, MinimumLength = 3)]
	public string? Username { get; set; }

	[EmailAddress]
	public string? Email { get; set; }

	[Phone]
	public string? PhoneNumber { get; set; }
}