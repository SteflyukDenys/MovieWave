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
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly ITokenGeneratorService _tokenGeneratorService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public AuthService(IBaseRepository<User> userRepository, ITokenGeneratorService tokenGeneratorService,
		ILogger logger, IMapper mapper, IBaseRepository<UserToken> userTokenRepository, IBaseRepository<Role> roleRepository, IUnitOfWork unitOfWork)
	{
		_userRepository = userRepository;
		_tokenGeneratorService = tokenGeneratorService;
		_logger = logger;
		_mapper = mapper;
		_userTokenRepository = userTokenRepository;
		_roleRepository = roleRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<BaseResult<UserDto>> RegisterAsync(RegisterUserDto dto)
	{
		if (dto.Password != dto.PasswordConfirm)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
				ErrorCode = (int)ErrorCodes.PasswordNotEqualsPasswordConfirm
			};
		}

		var user = await _userRepository.GetAll()
			.FirstOrDefaultAsync(u => u.Login == dto.Login);
		var userEmail = await _userRepository.GetAll()
			.FirstOrDefaultAsync(u => u.Email == dto.Email);
		if (user != null || userEmail != null)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.UserAlreadyExists,
				ErrorCode = (int)ErrorCodes.UserAlreadyExists
			};
		}

		var hashUserPassword = HashPassword(dto.Password);

		var transaction = await _unitOfWork.BeginTransactionAsync();
		using (transaction)
		{
			try
			{
				user = new User
				{
					UserName = dto.Email,
					Email = dto.Email,
					Login = dto.Login ?? dto.Email,
					PasswordHash = hashUserPassword
				};
				await _unitOfWork.Users.CreateAsync(user);

				await _unitOfWork.SaveChangesAsync();


				var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == nameof(Roles.User));
				if (role == null)
				{
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
				await _unitOfWork.UserRoles.CreateAsync(userRole);

				await _unitOfWork.SaveChangesAsync();

				await transaction.CommitAsync();
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
			}
		}

		return new BaseResult<UserDto>()
		{
			Data = _mapper.Map<UserDto>(user)
		};
	}

	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
	{
		var user = await _userRepository.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(u => u.Login == dto.Login);
		if (user == null)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InvalidLogin,
				ErrorCode = (int)ErrorCodes.InvalidLogin
			};
		}

		var isVerifyPassword = IsVerifyPassword(user.PasswordHash, dto.Password);
		if (!isVerifyPassword)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InvalidPassword,
				ErrorCode = (int)ErrorCodes.InvalidPassword
			};
		}

		var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserTokenId == user.Id);

		var userRoles = user.Roles;
		var claims = userRoles.Select(u => new Claim(ClaimTypes.Role, u.Name)).ToList();
		claims.Add(new Claim(ClaimTypes.Name, user.Login));

		var accessToken = _tokenGeneratorService.GenerateAccessToken(claims);
		var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

		if (userToken == null)
		{
			userToken = new UserToken()
			{
				UserTokenId = user.Id,
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
		return new BaseResult<TokenDto>()
		{
			Data = new TokenDto()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			}
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

		var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserTokenId == user.Id);
		if (userToken == null)
		{
			userToken = new UserToken
			{
				UserTokenId = user.Id,
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
