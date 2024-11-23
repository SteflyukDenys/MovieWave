using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Role;
using MovieWave.Domain.Dto.UserRole;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class RoleService : IRoleService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBaseRepository<User> _userRepository;
		private readonly IBaseRepository<Role> _roleRepository;
		private readonly IBaseRepository<UserRole> _userRoleRepository;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public RoleService(ILogger logger, IMapper mapper, IBaseRepository<User> userRepository,
			IBaseRepository<Role> roleRepository, IBaseRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_mapper = mapper;
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_userRoleRepository = userRoleRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<CollectionResult<RoleDto>> GetAllRolesAsync()
		{
			List<RoleDto> roles;

			roles = await _roleRepository.GetAll()
				.Select(r => _mapper.Map<RoleDto>(r))
				.ToListAsync();

			if (!roles.Any())
			{
				_logger.Warning(ErrorMessage.RoleNotFound);
				return new CollectionResult<RoleDto>
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RolesNotFound
				};
			}

			return new CollectionResult<RoleDto> { Data = roles, Count = roles.Count };
		}

		public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
		{
			var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
			if (role != null)
			{
				return new BaseResult<RoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleAlreadyExists,
					ErrorCode = (int)ErrorCodes.RoleAlreadyExists
				};
			}

			role = new Role()
			{
				Name = dto.Name
			};

			await _roleRepository.CreateAsync(role);
			await _roleRepository.SaveChangesAsync();

			return new BaseResult<RoleDto>()
			{
				Data = _mapper.Map<RoleDto>(role)
			};
		}

		public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
		{
			var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
			if (role == null)
			{
				return new BaseResult<RoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RoleNotFound
				};
			}

			role.Name = dto.Name;
			var updatedRole = _roleRepository.Update(role);
			await _roleRepository.SaveChangesAsync();

			return new BaseResult<RoleDto>()
			{
				Data = _mapper.Map<RoleDto>(updatedRole)
			};
		}

		public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
		{
			var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
			if (role == null)
			{
				return new BaseResult<RoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RoleNotFound
				};
			}

			_roleRepository.Remove(role);
			await _roleRepository.SaveChangesAsync();
			return new BaseResult<RoleDto>()
			{
				Data = _mapper.Map<RoleDto>(role)
			};
		}

		public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
		{
			var user = await _userRepository.GetAll()
				.Include(u => u.Roles)
				.FirstOrDefaultAsync(u => u.Login == dto.Login);

			if (user == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.UserNotFound,
					ErrorCode = (int)ErrorCodes.UserNotFound
				};
			}

			var roles = user.Roles.Select(ur => ur.Name).ToArray();
			if (roles.All(r => r != dto.RoleName))
			{
				var role = await _roleRepository.GetAll()
					.FirstOrDefaultAsync(r => r.Name == dto.RoleName);

				if (role == null)
				{
					return new BaseResult<UserRoleDto>()
					{
						ErrorMessage = ErrorMessage.RoleNotFound,
						ErrorCode = (int)ErrorCodes.RoleNotFound
					};
				}

				UserRole userRole = new UserRole()
				{
					RoleId = role.Id,
					UserId = user.Id
				};
				await _userRoleRepository.CreateAsync(userRole);
				await _userRoleRepository.SaveChangesAsync();
				return new BaseResult<UserRoleDto>()
				{
					Data = new UserRoleDto()
					{
						Login = user.Login,
						RoleName = role.Name
					}
				};
			}

			return new BaseResult<UserRoleDto>()
			{
				ErrorMessage = ErrorMessage.UserAlreadyExistsThisRole,
				ErrorCode = (int)ErrorCodes.UserAlreadyExistsThisRole
			};
		}


		public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
		{
			var user = await _userRepository.GetAll()
				.Include(u => u.Roles)
				.FirstOrDefaultAsync(u => u.Login == dto.Login);

			if (user == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.UserNotFound,
					ErrorCode = (int)ErrorCodes.UserNotFound
				};
			}

			var role = user.Roles.FirstOrDefault(r => r.Id == dto.FromRoleId);
			if (role == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RoleNotFound
				};
			}

			var newRoleForUser = await _roleRepository.GetAll().FirstOrDefaultAsync(u => u.Id == dto.ToRoleId);
			if (newRoleForUser == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RoleNotFound
				};
			}
			var transaction = await _unitOfWork.BeginTransactionAsync();
			using (transaction)
			{
				try
				{
					var userRole = await _unitOfWork.UserRoles.GetAll()
						.Where(ur => ur.RoleId == role.Id)
						.FirstAsync(ur => ur.UserId == user.Id);

					_unitOfWork.UserRoles.Remove(userRole);
					await _unitOfWork.SaveChangesAsync();

					var newUserRole = new UserRole()
					{
						UserId = user.Id,
						RoleId = newRoleForUser.Id
					};
					await _unitOfWork.UserRoles.CreateAsync(newUserRole);
					await _unitOfWork.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
				}
			}
			
			return new BaseResult<UserRoleDto>()
			{
				Data = new UserRoleDto()
				{
					Login = user.Login,
					RoleName = newRoleForUser.Name
				}
			};
		}


		public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto)
		{
			var user = await _userRepository.GetAll()
				.Include(u => u.Roles)
				.FirstOrDefaultAsync(u => u.Login == dto.Login);

			if (user == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.UserNotFound,
					ErrorCode = (int)ErrorCodes.UserNotFound
				};
			}

			var role = user.Roles.FirstOrDefault(r => r.Id == dto.RoleId);
			if (role == null)
			{
				return new BaseResult<UserRoleDto>()
				{
					ErrorMessage = ErrorMessage.RoleNotFound,
					ErrorCode = (int)ErrorCodes.RoleNotFound
				};
			}

			var userRole = await _userRoleRepository.GetAll()
				.Where(ur => ur.RoleId == role.Id)
				.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

			_userRoleRepository.Remove(userRole);
			_userRoleRepository.SaveChangesAsync();

			return new BaseResult<UserRoleDto>()
			{
				Data = new UserRoleDto()
				{
					Login = user.Login,
					RoleName = role.Name
				}
			};
		}

	}
}
