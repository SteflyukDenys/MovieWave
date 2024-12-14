using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class TagService : ITagService
{
	private readonly IBaseRepository<Tag> _tagRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IStorageService _storageService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public TagService(
		IBaseRepository<Tag> tagRepository, IStorageService storageService, ILogger logger, IMapper mapper, IUnitOfWork unitOfWork)
	{
		_tagRepository = tagRepository;
		_storageService = storageService;
		_logger = logger;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<CollectionResult<TagDto>> GetAllAsync()
	{
		var tags = await _tagRepository.GetAll()
			.Include(t => t.SeoAddition)
			.ToListAsync();

		if (!tags.Any())
		{
			_logger.Warning(ErrorMessage.TagsNotFound);
			return new CollectionResult<TagDto>
			{
				ErrorMessage = ErrorMessage.TagsNotFound,
				ErrorCode = (int)ErrorCodes.TagsNotFound
			};
		}

		var tagDtos = _mapper.Map<List<TagDto>>(tags);

		foreach (var tagDto in tagDtos)
		{
			if (!string.IsNullOrEmpty(tagDto.SeoAddition.MetaImagePath))
			{
				tagDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(tagDto.SeoAddition.MetaImagePath);
			}
		}

		return new CollectionResult<TagDto>
		{
			Data = tagDtos,
			Count = tagDtos.Count
		};
	}

	public async Task<BaseResult<TagDto>> GetByIdAsync(long id)
	{
		var tag = await _tagRepository.GetAll()
			.Include(t => t.SeoAddition)
			.FirstOrDefaultAsync(t => t.Id == id);

		if (tag == null)
		{
			_logger.Warning($"Tag {id} not found");
			return new BaseResult<TagDto>
			{
				ErrorMessage = ErrorMessage.TagNotFound,
				ErrorCode = (int)ErrorCodes.TagNotFound
			};
		}

		var tagDto = _mapper.Map<TagDto>(tag);

		if (!string.IsNullOrEmpty(tagDto.SeoAddition.MetaImagePath))
		{
			tagDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(tagDto.SeoAddition.MetaImagePath);
		}

		return new BaseResult<TagDto> { Data = tagDto };
	}

	public async Task<CollectionResult<Tag>> GetTagsByIdsAsync(List<long> tagIds)
	{
		var tags = await _tagRepository.GetAll()
			.Include(t => t.SeoAddition)
			.Where(t => tagIds.Contains(t.Id))
			.ToListAsync();

		if (tags == null || !tags.Any())
		{
			_logger.Warning("Теги не знайдено для наданих ідентифікаторів.");
			return new CollectionResult<Tag>
			{
				ErrorMessage = ErrorMessage.TagsNotFound,
				ErrorCode = (int)ErrorCodes.TagsNotFound
			};
		}

		return new CollectionResult<Tag>
		{
			Data = tags,
			Count = tags.Count
		};
	}

	public async Task<BaseResult<TagDto>> CreateAsync(CreateTagDto dto, FileDto imageUrl)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			if (imageUrl == null || imageUrl.Content.Length == 0)
			{
				return new BaseResult<TagDto>
				{
					ErrorMessage = ErrorMessage.InvalidFile,
					ErrorCode = 400
				};
			}

			var tag = _mapper.Map<Tag>(dto);

			// For S3
			var folder = $"tags";

			var uploadTagSeoImage = await _storageService.UploadFileAsync(imageUrl, folder);

			if (!uploadTagSeoImage.IsSuccess)
			{
				return new BaseResult<TagDto>
				{
					ErrorMessage = uploadTagSeoImage.ErrorMessage,
					ErrorCode = uploadTagSeoImage.ErrorCode
				};
			}

			tag.SeoAddition.MetaImagePath = uploadTagSeoImage.Data;

			await _tagRepository.CreateAsync(tag);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<TagDto>(tag);
			var result = resultDto.SeoAddition.MetaImagePath;
			resultDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(result);

			return new BaseResult<TagDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Помилка при створенні Tag: {Message}", ex.Message);

			return new BaseResult<TagDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<TagDto>> UpdateAsync(UpdateTagDto dto, FileDto newImageUrl)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var tag = await _tagRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == dto.Id);

			if (tag == null)
			{
				return new BaseResult<TagDto>()
				{
					ErrorMessage = ErrorMessage.TagNotFound,
					ErrorCode = (int)ErrorCodes.TagNotFound
				};
			}

			_mapper.Map(dto, tag);

			if (newImageUrl != null)
			{
				if (!string.IsNullOrEmpty(tag.SeoAddition.MetaImagePath))
				{
					var deleteResult = await _storageService.DeleteFileAsync(tag.SeoAddition.MetaImagePath);
					if (!deleteResult.IsSuccess)
					{
						_logger.Warning("Не вдалося видалити старий файл: {ErrorMessage}", deleteResult.ErrorMessage);
					}
				}

				var folder = $"tags";

				var uploadTagSeoImage = await _storageService.UploadFileAsync(newImageUrl, folder);
				if (!uploadTagSeoImage.IsSuccess)
				{
					return new BaseResult<TagDto>
					{
						ErrorMessage = uploadTagSeoImage.ErrorMessage,
						ErrorCode = uploadTagSeoImage.ErrorCode
					};
				}

				tag.SeoAddition.MetaImagePath = uploadTagSeoImage.Data;
			}


			_tagRepository.Update(tag);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<TagDto>(tag);
			var result = resultDto.SeoAddition.MetaImagePath;
			if (!string.IsNullOrEmpty(result))
			{
				resultDto.SeoAddition.MetaImagePath = _storageService.GenerateFileUrl(result);
			}

			return new BaseResult<TagDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error("Помилка при оновленні Tag: {Message}", ex.Message);
			return new BaseResult<TagDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<TagDto>> DeleteAsync(long id)
	{
		var tag = await _tagRepository.GetAll()
			.FirstOrDefaultAsync(t => t.Id == id);

		if (tag == null)
		{
			return new BaseResult<TagDto>
			{
				ErrorMessage = ErrorMessage.TagNotFound,
				ErrorCode = (int)ErrorCodes.TagNotFound
			};
		}

		if (!string.IsNullOrEmpty(tag.SeoAddition.MetaImagePath))
		{
			var deleteResult = await _storageService.DeleteFileAsync(tag.SeoAddition.MetaImagePath);
			if (!deleteResult.IsSuccess)
			{
				_logger.Error("Failed to delete MetaImagePath: {ErrorMessage}", deleteResult.ErrorMessage);
			}
		}

		_tagRepository.Remove(tag);
		await _tagRepository.SaveChangesAsync();

		return new BaseResult<TagDto>();
	}

	public async Task<List<long>> GetTagsByNamesAsync(List<string> names)
	{
		if (names == null || !names.Any())
			return new List<long>();

		var found = await _tagRepository.GetAll()
			.Where(t => names.Contains(t.Name))
			.Select(t => t.Id)
			.ToListAsync();
		return found;
	}
}
