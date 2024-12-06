using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class StudioService : IStudioService
{
	private readonly IBaseRepository<Studio> _studioRepository;
	private readonly IStorageService _storageService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public StudioService(IBaseRepository<Studio> studioRepository, ILogger logger, IMapper mapper,
		IStorageService storageService, IUnitOfWork unitOfWork)
	{
		_studioRepository = studioRepository;
		_logger = logger;
		_mapper = mapper;
		_storageService = storageService;
		_unitOfWork = unitOfWork;
	}

	public async Task<CollectionResult<StudioDto>> GetAllAsync()
	{
		List<StudioDto> studios;

		studios = await _studioRepository.GetAll()
			.Include(s => s.SeoAddition)
			.Select(s => _mapper.Map<StudioDto>(s))
			.ToListAsync();

		if (!studios.Any())
		{
			_logger.Warning(ErrorMessage.StudiosNotFound);
			return new CollectionResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudiosNotFound,
				ErrorCode = (int)ErrorCodes.StudiosNotFound
			};
		}

		return new CollectionResult<StudioDto> { Data = studios, Count = studios.Count };
	}

	public async Task<BaseResult<StudioDto>> GetByIdAsync(long id)
	{
		StudioDto? studioDto;

		var studio = await _studioRepository.GetAll()
			.Include(s => s.SeoAddition)
			.FirstOrDefaultAsync(s => s.Id == id);

		if (studio == null)
		{
			_logger.Warning($"Studio {id} not found");
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioNotFound,
				ErrorCode = (int)ErrorCodes.StudioNotFound
			};
		}

		studioDto = _mapper.Map<StudioDto>(studio);

		return new BaseResult<StudioDto> { Data = studioDto };
	}

	public async Task<CollectionResult<Studio>> GetStudioByIdsAsync(List<long> studioIds)
	{
		var studios = await _studioRepository.GetAll()
			.Include(t => t.SeoAddition)
			.Where(t => studioIds.Contains(t.Id))
			.ToListAsync();

		if (studios == null || !studios.Any())
		{
			_logger.Warning("Студії не знайдено для наданих ідентифікаторів.");
			return new CollectionResult<Studio>
			{
				ErrorMessage = ErrorMessage.StudiosNotFound,
				ErrorCode = (int)ErrorCodes.StudiosNotFound
			};
		}

		return new CollectionResult<Studio>
		{
			Data = studios,
			Count = studioIds.Count
		};
	}

	public async Task<BaseResult<StudioDto>> CreateAsync(CreateStudioDto dto, FileDto logoPath)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var existingStudio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Name == dto.Name);
			if (existingStudio != null)
			{
				return new BaseResult<StudioDto>
				{
					ErrorMessage = ErrorMessage.StudioAlreadyExists,
					ErrorCode = (int)ErrorCodes.StudioAlreadyExists
				};
			}

			var newStudio = _mapper.Map<Studio>(dto);
			var folder = $"studios";

			var uploadLogoImage = await _storageService.UploadFileAsync(logoPath, folder);

			if (!uploadLogoImage.IsSuccess)
			{
				return new BaseResult<StudioDto>
				{
					ErrorMessage = uploadLogoImage.ErrorMessage,
					ErrorCode = uploadLogoImage.ErrorCode
				};
			}

			newStudio.LogoPath = uploadLogoImage.Data;
			newStudio.SeoAddition.MetaImagePath = uploadLogoImage.Data;

			await _studioRepository.CreateAsync(newStudio);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<StudioDto>(newStudio);

			resultDto.LogoPath = _storageService.GenerateFileUrl(resultDto.LogoPath);
			resultDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(resultDto.LogoPath);

			return new BaseResult<StudioDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Помилка при додавання Studio: {Message}", ex.Message);

			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<StudioDto>> UpdateAsync(UpdateStudioDto dto, FileDto newLogoPath)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();
		try
		{
			var studio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Id == dto.Id);
			if (studio == null)
			{
				return new BaseResult<StudioDto>
				{
					ErrorMessage = ErrorMessage.StudioNotFound,
					ErrorCode = (int)ErrorCodes.StudioNotFound
				};
			}

			_mapper.Map(dto, studio);

			if (newLogoPath != null)
			{
				if (!string.IsNullOrEmpty(studio.LogoPath))
				{
					var deleteResult = await _storageService.DeleteFileAsync(studio.LogoPath);
					var deleteSeo = await _storageService.DeleteFileAsync(studio.SeoAddition.MetaImagePath);

					if (!deleteResult.IsSuccess || !deleteSeo.IsSuccess)
					{
						_logger.Warning("Не вдалося видалити старий логотип: {ErrorMessage}", deleteResult.ErrorMessage);
					}
				}

				var folder = $"studios";

				var uploadStudioLogo = await _storageService.UploadFileAsync(newLogoPath, folder);
				if (!uploadStudioLogo.IsSuccess)
				{
					return new BaseResult<StudioDto>
					{
						ErrorMessage = uploadStudioLogo.ErrorMessage,
						ErrorCode = uploadStudioLogo.ErrorCode
					};
				}

				studio.LogoPath = uploadStudioLogo.Data;
				studio.SeoAddition.MetaImagePath = uploadStudioLogo.Data;
			}

			_studioRepository.Update(studio);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<StudioDto>(studio);

			if (!string.IsNullOrEmpty(resultDto.LogoPath))
			{
				resultDto.LogoPath = _storageService.GenerateFileUrl(resultDto.LogoPath);
				resultDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(resultDto.LogoPath);
			}

			return new BaseResult<StudioDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error("Помилка при оновленні логотип студії: {Message}", ex.Message);
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<StudioDto>> DeleteAsync(long id)
	{
		var studio = await _studioRepository.GetAll().FirstOrDefaultAsync(s => s.Id == id);
		if (studio == null)
		{
			return new BaseResult<StudioDto>
			{
				ErrorMessage = ErrorMessage.StudioNotFound,
				ErrorCode = (int)ErrorCodes.StudioNotFound
			};
		}

		_studioRepository.Remove(studio);
		await _studioRepository.SaveChangesAsync();

		return new BaseResult<StudioDto> { Data = _mapper.Map<StudioDto>(studio) };
	}
}
