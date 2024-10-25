using MovieWave.Domain.Dto.User;

namespace MovieWave.Domain.Dto;

public class TokenDto
{
	public string AccessToken { get; set; }
	public string RefreshToken { get; set; }
	public UserDto User { get; set; }
}