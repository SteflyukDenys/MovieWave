using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using MovieWave.Domain.Settings;
using Serilog;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MovieWave.Application.Resources;
using AutoMapper;

namespace MovieWave.Application.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<User> _userManager;
	private readonly IJwtTokenGeneratorService _jwtTokenGenerator;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public AuthService(UserManager<User> userManager, IJwtTokenGeneratorService jwtTokenGenerator, ILogger logger, IMapper mapper)
	{
		_userManager = userManager;
		_jwtTokenGenerator = jwtTokenGenerator;
		_logger = logger;
		_mapper = mapper;
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

		var existingUser = await _userManager.FindByNameAsync(dto.Login);
		if (existingUser != null)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.UserAlreadyExists,
				ErrorCode = (int)ErrorCodes.UserAlreadyExists
			};
		}

		var user = new User
		{
			UserName = dto.Login,
			Email = dto.Email,
			Login = dto.Login,
			UserRole = UserRole.User
		};

		var result = await _userManager.CreateAsync(user, dto.Password);
		if (result.Succeeded)
		{
			var userDto = _mapper.Map<UserDto>(user);
			return new BaseResult<UserDto> { Data = userDto };
		}
		else
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)),
				ErrorCode = (int)ErrorCodes.UserCreationFailed
			};
		}
	}

	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
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

		var result = await _userManager.CheckPasswordAsync(user, dto.Password);
		if (!result)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InvalidPassword,
				ErrorCode = (int)ErrorCodes.InvalidPassword
			};
		}

		var token = _jwtTokenGenerator.GenerateToken(user);
		var userDto = _mapper.Map<UserDto>(user);

		return new BaseResult<TokenDto>
		{
			Data = new TokenDto
			{
				AccessToken = token,
				User = userDto
			}
		};
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

			var token = _jwtTokenGenerator.GenerateToken(user);
			var userDto = _mapper.Map<UserDto>(user);

			return new BaseResult<TokenDto>
			{
				Data = new TokenDto
				{
					AccessToken = token,
					User = userDto
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
}
