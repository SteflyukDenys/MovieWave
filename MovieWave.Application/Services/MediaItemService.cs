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
				.Select(x => new MediaItemDto(x.Id, x.Name, x.OriginalName, x.Description,
				x.PosterPath, x.Duration, x.EpisodesCount, x.ImdbScore, x.CreatedAt.ToLongDateString()))
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
			mediaItem = await _mediaItemRepository.GetAll()
				.Select(x => new MediaItemDto(x.Id, x.Name, x.OriginalName, x.Description,
				x.PosterPath, x.Duration, x.EpisodesCount, x.ImdbScore, x.CreatedAt.ToLongDateString()))
				.FirstOrDefaultAsync(x => x.Id == id);
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

		if (mediaItem == null)
		{
			_logger.Warning("MediaItem {Id} not found", id);
			return new BaseResult<MediaItemDto>()
			{
				ErrorMessage = ErrorMessage.MediaItemNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemNotFound

			};
		}

		return new BaseResult<MediaItemDto>()
		{
			Data = mediaItem
		};

	}

	public async Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto)
	{
		try
		{
			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
			var result = _mediaItemValidator.CreateValidator(mediaItem);

			if (!result.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = result.ErrorMessage,
					ErrorCode = result.ErrorCode
				};
			}

			mediaItem = new MediaItem()
			{
				Id = Guid.NewGuid(),
				Name = dto.Name,
				OriginalName = dto.OriginalName,
				Description = dto.Description,
				PosterPath = dto.PosterPath,
				Duration = dto.Duration,
				EpisodesCount = dto.EpisodesCount,
				ImdbScore = dto.ImdbScore
			};
			await _mediaItemRepository.CreateAsync(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			return new BaseResult<MediaItemDto>()
			{
				Data = _mapper.Map<MediaItemDto>(mediaItem)
			};
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
			var result = _mediaItemValidator.ValidateOnNull(mediaItem);

			if (!result.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = result.ErrorMessage,
					ErrorCode = result.ErrorCode
				};
			}

			_mediaItemRepository.Remove(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();
			return new BaseResult<MediaItemDto>()
			{
				Data = _mapper.Map<MediaItemDto>(mediaItem)
			};
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
			var result = _mediaItemValidator.ValidateOnNull(mediaItem);

			if (!result.IsSuccess)
			{
				return new BaseResult<MediaItemDto>()
				{
					ErrorMessage = result.ErrorMessage,
					ErrorCode = result.ErrorCode
				};
			}

			mediaItem.Name = dto.Name;
			mediaItem.OriginalName = dto.OriginalName;
			mediaItem.Description = dto.Description;
			mediaItem.PosterPath = dto.PosterPath;
			mediaItem.Duration = dto.Duration;
			mediaItem.EpisodesCount = dto.EpisodesCount;
			mediaItem.ImdbScore = dto.ImdbScore;

			var updatedMediaItem = _mediaItemRepository.Update(mediaItem);
			await _mediaItemRepository.SaveChangesAsync();

			return new BaseResult<MediaItemDto>()
			{
				Data = _mapper.Map<MediaItemDto>(updatedMediaItem)
			};

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
		throw new NotImplementedException();
	}
}
