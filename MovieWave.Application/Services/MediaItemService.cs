using System;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Interfaces.Validations;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class MediaItemService : IMediaItemService
{
	private readonly IBaseRepository<MediaItem> _mediaItemRepository;
	private readonly ILogger _logger;
	private readonly IMediaItemValidator _mediaItemValidator;
	private readonly IMapper _mapper;

	public MediaItemService(IBaseRepository<MediaItem> mediaItemRepository, ILogger logger,
		IMediaItemValidator mediaItemValidator, IMapper mapper)
	{
		_mediaItemRepository = mediaItemRepository;
		_logger = logger;
		_mediaItemValidator = mediaItemValidator;
		_mapper = mapper;
	}

	public async Task<CollectionResult<MediaItemDto>> GetMediaItemsAsync()
	{
		List<MediaItemDto> mediaItems;
		try
		{
			mediaItems = await _mediaItemRepository.GetAll()
				.Include(x => x.MediaItemType)
				.Include(x => x.Status)
				.Include(x => x.RestrictedRating)
				.Include(x => x.Episodes)
				.Include(x => x.Attachments)
				.Include(x => x.Reviews)
				.Include(x => x.Notifications)
				.Include(x => x.Comments)
				.Include(x => x.Countries)
				.Include(x => x.Studios)
				.Include(x => x.Tags)
				.Include(x => x.People)
				.Select(x => _mapper.Map<MediaItemDto>(x))
				.ToListAsync();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new CollectionResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
		if (!mediaItems.Any())
		{
			_logger.Warning(ErrorMessage.MediaItemsNotFound, mediaItems.Count);
			return new CollectionResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.MediaItemsNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemsNotFound
			};
		}

		return new CollectionResult<MediaItemDto>()
		{
			Data = mediaItems,
			Count = mediaItems.Count
		};
	}

	public async Task<BaseResult<MediaItemDto>> GetMediaItemByIdAsync(Guid id)
	{
		MediaItemDto? mediaItem;

		try
		{
			var mediaItemEntity = await _mediaItemRepository.GetAll()
				.Include(x => x.MediaItemType)
				.Include(x => x.Status)
				.Include(x => x.RestrictedRating)
				.Include(x => x.Episodes)
				.Include(x => x.Attachments)
				.Include(x => x.Reviews)
				.Include(x => x.Notifications)
				.Include(x => x.Comments)
				.Include(x => x.Countries)
				.Include(x => x.Studios)
				.Include(x => x.Tags)
				.Include(x => x.People)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (mediaItemEntity == null)
			{
				_logger.Warning($"MediaItem {id} not found", id);
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			mediaItem = _mapper.Map<MediaItemDto>(mediaItemEntity);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}

		return new BaseResult<MediaItemDto>() { Data = mediaItem };
	}

	public async Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto)
	{
		try
		{
			var existingMediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
			var validationResult = _mediaItemValidator.CreateValidator(existingMediaItem);

			if (!validationResult.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = validationResult.ErrorMessage,
					ErrorCode = validationResult.ErrorCode
				};
			}

			var newMediaItem = _mapper.Map<MediaItem>(dto);
			await _mediaItemRepository.CreateAsync(newMediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			return new BaseResult<MediaItemDto>() { Data = _mapper.Map<MediaItemDto>(newMediaItem) };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<MediaItemDto>> DeleteMediaItemAsync(Guid id)
	{
		try
		{
			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
			var validationResult = _mediaItemValidator.ValidateOnNull(mediaItem);

			if (!validationResult.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = validationResult.ErrorMessage,
					ErrorCode = validationResult.ErrorCode
				};
			}

			_mediaItemRepository.Remove(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			return new BaseResult<MediaItemDto>() { Data = _mapper.Map<MediaItemDto>(mediaItem) };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<BaseResult<MediaItemDto>> UpdateMediaItemAsync(UpdateMediaItemDto dto)
	{
		try
		{
			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.id);
			var validationResult = _mediaItemValidator.ValidateOnNull(mediaItem);

			if (!validationResult.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = validationResult.ErrorMessage,
					ErrorCode = validationResult.ErrorCode
				};
			}

			_mapper.Map(dto, mediaItem);
			var updatedMediaItem = _mediaItemRepository.Update(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			return new BaseResult<MediaItemDto>() { Data = _mapper.Map<MediaItemDto>(updatedMediaItem) };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return new BaseResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}
}
