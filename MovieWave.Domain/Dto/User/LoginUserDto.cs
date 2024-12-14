using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Dto.User;

public class LoginUserDto
{
	[Required]
	public string EmailOrPhone { get; set; }

	[Required]
	public string Password { get; set; }
}