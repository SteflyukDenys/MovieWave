using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;
using System.Net.Mail;

namespace MovieWave.Application.Services
{
	public class AccountService : IAccountService
	{
		private readonly IBaseRepository<User> _userRepository;
		private readonly IBaseRepository<UserRole> _userRoleRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IStorageService _storageService; 

		public AccountService(
			IBaseRepository<User> userRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			ILogger logger,
			IStorageService storageService, IBaseRepository<UserRole> userRoleRepository)
		{
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
			_storageService = storageService;
			_userRoleRepository = userRoleRepository;
		}

		public async Task<BaseResult<UserDto>> UpdateProfileAsync(Guid userId, UpdateUserDto dto)
		{
			var user = await _userRepository.GetAll()
				.Include(u => u.Roles)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
			{
				return new BaseResult<UserDto>
				{
					ErrorMessage = ErrorMessage.UserNotFound,
					ErrorCode = (int)ErrorCodes.UserNotFound
				};
			}

			if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.UserName)
			{
				var existingUser = await _userRepository.GetAll()
					.FirstOrDefaultAsync(u => u.UserName == dto.Username);
				if (existingUser != null)
				{
					return new BaseResult<UserDto>
					{
						ErrorMessage = ErrorMessage.UserAlreadyExists,
						ErrorCode = (int)ErrorCodes.UserAlreadyExists
					};
				}
				user.UserName = dto.Username;
				user.Login = dto.Username;
			}

			if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
			{
				var existingEmailUser = await _userRepository.GetAll()
					.FirstOrDefaultAsync(u => u.Email == dto.Email);
				if (existingEmailUser != null)
				{
					return new BaseResult<UserDto>
					{
						ErrorMessage = ErrorMessage.EmailAlreadyUse,
						ErrorCode = (int)ErrorCodes.UserAlreadyExists
					};
				}
				user.Email = dto.Email;
			}

			if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != user.PhoneNumber)
			{
				var existingPhoneUser = await _userRepository.GetAll()
					.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
				if (existingPhoneUser != null)
				{
					return new BaseResult<UserDto>
					{
						ErrorMessage = ErrorMessage.PhoneAalreadyUse,
						ErrorCode = (int)ErrorCodes.UserAlreadyExists
					};
				}
				user.PhoneNumber = dto.PhoneNumber;
			}
			var userDto = _mapper.Map<UserDto>(user);
			userDto.AvatarPath = _storageService.GenerateFileUrl(user.AvatarPath);

			await _unitOfWork.SaveChangesAsync();

			return new BaseResult<UserDto>
			{
				Data = userDto
			};
		}

		public async Task<BaseResult<UserDto>> UpdateAvatarAsync(Guid userId, FileDto avatar)
		{
			using var transaction = await _unitOfWork.BeginTransactionAsync();

			try
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

				if (avatar == null || avatar.Content.Length == 0)
				{
					return new BaseResult<UserDto>
					{
						ErrorMessage = ErrorMessage.InvalidFile,
						ErrorCode = 400
					};
				}

				var folder = $"users/{userId}/avatar";
				var uploadUserAvatar = await _storageService.UploadFileAsync(avatar, folder);

				if (!uploadUserAvatar.IsSuccess)
				{
					return new BaseResult<UserDto>
					{
						ErrorMessage = uploadUserAvatar.ErrorMessage,
						ErrorCode = uploadUserAvatar.ErrorCode
					};
				}

				user.AvatarPath = uploadUserAvatar.Data;
				user.UpdatedAt = DateTime.UtcNow;
				user.UpdatedBy = 1;

				_userRepository.Update(user);
				await _unitOfWork.SaveChangesAsync();
				await transaction.CommitAsync();

				var resultDto = _mapper.Map<UserDto>(user);

				resultDto.AvatarPath = _storageService.GenerateFileUrl(resultDto.AvatarPath);

				return new BaseResult<UserDto> { Data = resultDto };
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				_logger.Error(ex, "Помилка при додавані аватарки: {Message}", ex.Message);

				return new BaseResult<UserDto>
				{
					ErrorMessage = ErrorMessage.InternalServerError,
					ErrorCode = (int)ErrorCodes.InternalServerError
				};
			}
		}

		public async Task<BaseResult<UserDto>> GetUserByIdAsync(Guid userId)
		{
			var user = await _userRepository.GetAll()
				.Include(u => u.Roles)
				.ThenInclude(ur => ur.Users)
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
			userDto.AvatarPath = _storageService.GenerateFileUrl(user.AvatarPath);
			return new BaseResult<UserDto>
			{
				Data = userDto
			};
		}

	}
}
