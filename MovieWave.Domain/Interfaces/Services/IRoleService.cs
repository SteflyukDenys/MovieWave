using MovieWave.Domain.Dto.Role;
using MovieWave.Domain.Dto.UserRole;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IRoleService
{
	Task<CollectionResult<RoleDto>> GetAllRolesAsync();

	Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto);

	Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto);

	Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);

	Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto);

	Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto);

	Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto);

}