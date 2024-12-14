using System.Security.Claims;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using MovieWave.Application.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Databases;

namespace MovieWave.Application.Services;

public class AuthService : IAuthService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBaseRepository<User> _userRepository;
	private readonly IBaseRepository<Role> _roleRepository;
	private readonly IBaseRepository<UserRole> _userRoleRepository;
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly ITokenGeneratorService _tokenGeneratorService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public AuthService(IBaseRepository<User> userRepository, ITokenGeneratorService tokenGeneratorService,
		ILogger logger, IMapper mapper, IBaseRepository<UserToken> userTokenRepository, IBaseRepository<Role> roleRepository, IUnitOfWork unitOfWork, IBaseRepository<UserRole> userRoleRepository)
	{
		_userRepository = userRepository;
		_tokenGeneratorService = tokenGeneratorService;
		_logger = logger;
		_mapper = mapper;
		_userTokenRepository = userTokenRepository;
		_roleRepository = roleRepository;
		_unitOfWork = unitOfWork;
		_userRoleRepository = userRoleRepository;
	}

	public async Task<BaseResult<UserDto>> RegisterAsync(RegisterUserDto dto)
	{
		var existingUser = await _userRepository.GetAll()
			.FirstOrDefaultAsync(u => u.Login == dto.Username);

		if (existingUser != null)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.UserAlreadyExists,
				ErrorCode = (int)ErrorCodes.UserAlreadyExists
			};
		}

		var hashedPassword = HashPassword(dto.Password);

		var transaction = await _unitOfWork.BeginTransactionAsync();

		using (transaction)
		{
			try
			{
				var user = new User
				{
					UserName = dto.Username,
					Email = dto.Email,
					PhoneNumber = dto.PhoneNumber,
					Login = dto.Username,
					PasswordHash = hashedPassword,
					CreatedAt = DateTime.UtcNow,
					CreatedBy = 1 
				};
				await _userRepository.CreateAsync(user);
				await _unitOfWork.SaveChangesAsync();

				var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == nameof(Roles.User));
				if (role == null)
				{
					await transaction.RollbackAsync();
					return new BaseResult<UserDto>()
					{
						ErrorMessage = ErrorMessage.RoleNotFound,
						ErrorCode = (int)ErrorCodes.RoleNotFound
					};
				}

				UserRole userRole = new UserRole()
				{
					UserId = user.Id,
					RoleId = role.Id
				};
				await _userRoleRepository.CreateAsync(userRole);
				await _unitOfWork.SaveChangesAsync();

				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.UserName)
					};

				foreach (var userRoleName in user.UserRoles.Select(ur => ur.Role.Name))
				{
					claims.Add(new Claim(ClaimTypes.Role, userRoleName));
				}

				var accessToken = _tokenGeneratorService.GenerateAccessToken(claims);
				var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

				user.UserToken = new UserToken
				{
					RefreshToken = refreshToken,
					RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
					UserId = user.Id
				};

				_userRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();

				await transaction.CommitAsync();

				var userDto = _mapper.Map<UserDto>(user);
				return new BaseResult<UserDto>()
				{
					Data = userDto
				};
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error(ex, "Error registering user: {Message}", ex.Message);
				return new BaseResult<UserDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}
	}

	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
	{
		var user = await _userRepository.GetAll()
			.Include(u => u.UserRoles)
			.ThenInclude(ur => ur.Role)
			.Include(u => u.UserToken)
			.FirstOrDefaultAsync(u => u.Email == dto.EmailOrPhone || u.PhoneNumber == dto.EmailOrPhone);

		if (user == null || !IsVerifyPassword(user.PasswordHash, dto.Password))
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InvalidLogin,
				ErrorCode = (int)ErrorCodes.InvalidLogin
			};
		}

		var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName)
		};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var accessToken = _tokenGeneratorService.GenerateAccessToken(claims);
		var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

		if (user.UserToken == null)
		{
			user.UserToken = new UserToken
			{
				RefreshToken = refreshToken,
				RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
				UserId = user.Id
			};
		}
		else
		{
			user.UserToken.RefreshToken = refreshToken;
			user.UserToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
		}

		_userRepository.Update(user);
		await _unitOfWork.SaveChangesAsync();

		var response = new TokenDto()
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};

		return new BaseResult<TokenDto>
		{
			Data = response
		};
	}


	public async Task<BaseResult<UserDto>> GetUserByIdAsync(Guid userId)
	{
		var user = await _userRepository.GetAll()
			.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
			.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.UserNotFound,
				ErrorCode = (int)ErrorCodes.UserNotFound
			};
		}

		var userDto = _mapper.Map<UserDto>(user);
		return new BaseResult<UserDto>
		{
			Data = userDto
		};
	}

	public async Task<BaseResult<TokenDto>> ExternalLoginAsync(string email, string name)
	{
		var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == email);
		if (user == null)
		{
			user = new User
			{
				UserName = email,
				Email = email,
				Login = name ?? email,
				EmailConfirmed = true
			};

			await _userRepository.CreateAsync(user);
			await _userRepository.SaveChangesAsync();
		}

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Login),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, nameof(Roles.User))
		};
		var accessToken = _tokenGeneratorService.GenerateAccessToken(claims);
		var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

		var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);
		if (userToken == null)
		{
			userToken = new UserToken
			{
				UserId = user.Id,
				RefreshToken = refreshToken,
				RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
			};
			await _userTokenRepository.CreateAsync(userToken);
		}
		else
		{
			userToken.RefreshToken = refreshToken;
			userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
			_userTokenRepository.Update(userToken);
		}

		await _userTokenRepository.SaveChangesAsync();
		return new BaseResult<TokenDto>
		{
			Data = new TokenDto
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			}
		};
	}

	private string HashPassword(string password)
	{
		using var sha256 = SHA256.Create();
		var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
		var builder = new StringBuilder();
		foreach (var b in bytes)
		{
			builder.Append(b.ToString("x2"));
		}
		return builder.ToString();
	}

	private bool IsVerifyPassword(string hash, string password)
	{
		var hashedInput = HashPassword(password);
		return hash == hashedInput;
	}
}
