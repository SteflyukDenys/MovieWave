using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class PersonImageService : IPersonImageService
{
	private readonly IStorageService _storageService;
	private readonly IBaseRepository<PersonImage> _personImageRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ILogger _logger;

	public PersonImageService(
		IBaseRepository<PersonImage> personImageRepository,
		IStorageService storageService,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		ILogger logger)
	{
		_personImageRepository = personImageRepository;
		_storageService = storageService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<BaseResult<PersonImageDto>> UploadPersonImageAsync(CreatePersonImageDto dto, FileDto uploadImage)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			if (uploadImage == null || uploadImage.Content.Length == 0)
			{
				return new BaseResult<PersonImageDto>
				{
					ErrorMessage = ErrorMessage.InvalidFile,
					ErrorCode = 400
				};
			}

			var personImage = _mapper.Map<PersonImage>(dto);
			var imageType = Enum.GetName(typeof(PersonImageType), dto.ImageType);
			var folder = $"people/{dto.PersonId}/{imageType}";

			var uploadResult = await _storageService.UploadFileAsync(uploadImage, folder);

			if (!uploadResult.IsSuccess)
			{
				return new BaseResult<PersonImageDto>
				{
					ErrorMessage = uploadResult.ErrorMessage,
					ErrorCode = uploadResult.ErrorCode
				};
			}

			personImage.ImagePath = uploadResult.Data;

			await _personImageRepository.CreateAsync(personImage);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<PersonImageDto>(personImage);
			resultDto.ImagePath = _storageService.GenerateFileUrl(resultDto.ImagePath);

			return new BaseResult<PersonImageDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Виникла помилка при додавані картинки: {Message}", ex.Message);

			if (transaction != null && transaction.GetDbTransaction().Connection != null)
			{
				await transaction.RollbackAsync();
			}

			return new BaseResult<PersonImageDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<PersonImageDto>> GetPersonImageByIdAsync(Guid personImageId)
	{
		var personImage = await _personImageRepository.GetAll()
			.FirstOrDefaultAsync(pi => pi.Id == personImageId);

		if (personImage == null)
		{
			_logger.Warning("PersonImage з ID {personImageId} не знайдено.", personImageId);
			return new BaseResult<PersonImageDto>
			{
				ErrorMessage = ErrorMessage.PersonImageNotFound,
				ErrorCode = (int)ErrorCodes.PersonImageNotFound
			};
		}

		var personImageDto = _mapper.Map<PersonImageDto>(personImage);
		personImageDto.ImagePath = _storageService.GenerateFileUrl(personImageDto.ImagePath);

		return new BaseResult<PersonImageDto> { Data = personImageDto };
	}

	public async Task<CollectionResult<PersonImageDto>> GetImagesByPersonIdAsync(Guid personId)
	{
		var personImages = await _personImageRepository.GetAll()
			.Where(pi => pi.PersonId == personId)
			.ToListAsync();

		var personImagesDtos = personImages.Select(pi =>
		{
			var dto = _mapper.Map<PersonImageDto>(pi);
			dto.ImagePath = _storageService.GenerateFileUrl(pi.ImagePath);
			return dto;
		}).ToList();

		return new CollectionResult<PersonImageDto>
		{
			Data = personImagesDtos,
			Count = personImagesDtos.Count
		};
	}

	public async Task<BaseResult<PersonImageDto>> UpdatePersonImageAsync(UpdatePersonImageDto dto, FileDto newImage)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var personImage = await _personImageRepository.GetAll()
				.FirstOrDefaultAsync(pi => pi.Id == dto.Id);

			if (personImage == null)
			{
				return new BaseResult<PersonImageDto>
				{
					ErrorMessage = ErrorMessage.PersonImageNotFound,
					ErrorCode = (int)ErrorCodes.PersonImageNotFound
				};
			}

			if (newImage != null)
			{
				if (!string.IsNullOrEmpty(personImage.ImagePath))
				{
					var deleteResult = await _storageService.DeleteFileAsync(personImage.ImagePath);
					if (!deleteResult.IsSuccess)
					{
						_logger.Warning("Не вдалося видалити старий файл: {ErrorMessage}", deleteResult.ErrorMessage);
					}
				}

				var imageType = Enum.GetName(typeof(PersonImageType), dto.ImageType);
				var folder = $"people/{personImage.PersonId}/{imageType}";

				var uploadResult = await _storageService.UploadFileAsync(newImage, folder);

				if (!uploadResult.IsSuccess)
				{
					return new BaseResult<PersonImageDto>
					{
						ErrorMessage = uploadResult.ErrorMessage,
						ErrorCode = uploadResult.ErrorCode
					};
				}

				personImage.ImagePath = uploadResult.Data;
			}

			_mapper.Map(dto, personImage);
			_personImageRepository.Update(personImage);

			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<PersonImageDto>(personImage);
			resultDto.ImagePath = _storageService.GenerateFileUrl(resultDto.ImagePath);

			return new BaseResult<PersonImageDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Помилка при оновленні PersonImage: {Message}", ex.Message);
			return new BaseResult<PersonImageDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult> DeletePersonImageAsync(Guid personImageId)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var personImage = await _personImageRepository.GetAll()
				.FirstOrDefaultAsync(pi => pi.Id == personImageId);

			if (personImage == null)
			{
				_logger.Warning("PersonImage з ID {personImageId} не знайдено.", personImageId);
				return new BaseResult
				{
					ErrorMessage = ErrorMessage.PersonImageNotFound,
					ErrorCode = (int)ErrorCodes.PersonImageNotFound
				};
			}

			var deleteResult = await _storageService.DeleteFileAsync(personImage.ImagePath);

			if (!deleteResult.IsSuccess)
			{
				_logger.Error("Не вдалося видалити файл зі сховища: {ErrorMessage}", deleteResult.ErrorMessage);
				await transaction.RollbackAsync();
				return new BaseResult
				{
					ErrorMessage = deleteResult.ErrorMessage,
					ErrorCode = deleteResult.ErrorCode
				};
			}

			_personImageRepository.Remove(personImage);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			return new BaseResult();
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Помилка при видаленні PersonImage: {Message}", ex.Message);
			return new BaseResult
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}
}
