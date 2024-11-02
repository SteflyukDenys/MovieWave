using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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

namespace MovieWave.Application.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<User> _userManager;
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly ITokenGeneratorService _tokenGeneratorService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public AuthService(UserManager<User> userManager, ITokenGeneratorService tokenGeneratorService,
		ILogger logger, IMapper mapper, IBaseRepository<UserToken> userTokenRepository)
	{
		_userManager = userManager;
		_tokenGeneratorService = tokenGeneratorService;
		_logger = logger;
		_mapper = mapper;
		_userTokenRepository = userTokenRepository;
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

		try
		{
			var existingUser = await _userManager.FindByNameAsync(dto.Login);
			var existingUserEmail = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null || existingUserEmail != null)
			{
				return new BaseResult<UserDto>
				{
					ErrorMessage = ErrorMessage.UserAlreadyExists,
					ErrorCode = (int)ErrorCodes.UserAlreadyExists
				};
			}

			var hashUserPassword = HashPassword(dto.Password);
			var user = new User
			{
				UserName = dto.Login,
				Login = dto.Login,
				Email = dto.Email,
				PasswordHash = hashUserPassword,
				UserRole = UserRole.User
			};

			var result = await _userManager.CreateAsync(user);
			if (!result.Succeeded)
			{
				return new BaseResult<UserDto>
				{
					ErrorMessage = ErrorMessage.FailedToCreateUser,
					ErrorCode = (int)ErrorCodes.UserCreationFailed
				};
			}

			return new BaseResult<UserDto>()
			{
				Data = _mapper.Map<UserDto>(user)
			};
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<UserDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
	{
		try
		{
			var user = await _userManager.FindByNameAsync(dto.Login);
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

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, user.UserRole.ToString())
			};
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
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);
			return new BaseResult<TokenDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<TokenDto>> ExternalLoginAsync(string email, string name)
	{
		try
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				user = new User
				{
					UserName = email,
					Email = email,
					Login = name ?? email,
					UserRole = UserRole.User,
					EmailConfirmed = true
				};

				var result = await _userManager.CreateAsync(user);
				if (!result.Succeeded)
				{
					return new BaseResult<TokenDto>
					{
						ErrorMessage = ErrorMessage.FailedToCreateUser,
						ErrorCode = (int)ErrorCodes.UserCreationFailed
					};
				}
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.UserRole.ToString())
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
			}

			var userDto = _mapper.Map<UserDto>(user);
			return new BaseResult<TokenDto>
			{
				Data = new TokenDto
				{
					AccessToken = accessToken,
					RefreshToken = refreshToken
				}
			};
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error during external login.");
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}


	private string HashPassword(string password)
	{
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
		return Convert.ToBase64String(bytes);
	}

	private bool IsVerifyPassword(string userPasswordHash, string userPassword)
	{
		var hash = HashPassword(userPassword);

		return userPasswordHash == hash;
	}
}
