using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IAuthService
{
	Task<BaseResult<UserDto>> RegisterAsync(RegisterUserDto dto);
	Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto);
	Task<BaseResult<TokenDto>> ExternalLoginAsync(string email, string name);
}
