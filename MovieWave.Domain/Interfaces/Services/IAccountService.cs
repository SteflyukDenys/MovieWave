using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IAccountService
{
	Task<BaseResult<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserDto dto);

	Task<BaseResult<UserDto>> UpdateAvatarAsync(Guid userId, FileDto avatar);

	Task<BaseResult<UserDto>> GetUserByIdAsync(Guid userId);
}