using System;
using System.Linq;
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

		return new BaseResult<MediaItemDto>() { Data = mediaItem };
	}

	public async Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto)
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

	public async Task<BaseResult<MediaItemDto>> DeleteMediaItemAsync(Guid id)
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

	public async Task<BaseResult<MediaItemDto>> UpdateMediaItemAsync(UpdateMediaItemDto dto)
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

	public async Task<CollectionResult<MediaItemDto>> SearchMediaItemsAsync(MediaItemSearchDto searchDto)
	{
		var query = _mediaItemRepository.GetAll()
			.Include(mi => mi.Tags)
			.Include(mi => mi.Status)
			.Include(mi => mi.MediaItemType)
			.AsQueryable();

		// Search by name and description
		if (!string.IsNullOrEmpty(searchDto.Query))
		{
			query = query.Where(mi => EF.Functions.ToTsVector("russian", mi.Name + " " + mi.Description)
				.Matches(EF.Functions.PlainToTsQuery("russian", searchDto.Query)));
		}

		// Filter by tags
		if (searchDto.TagIds != null && searchDto.TagIds.Any())
		{
			query = query.Where(mi => mi.Tags.Any(t => searchDto.TagIds.Contains(t.Id)));
		}

		// Filter by status
		if (searchDto.StatusId.HasValue)
		{
			query = query.Where(mi => mi.StatusId == searchDto.StatusId.Value);
		}

		// Filter by type of media element
		if (searchDto.MediaTypeId.HasValue)
		{
			query = query.Where(mi => mi.MediaItemTypeId == searchDto.MediaTypeId.Value);
		}

		// Sorting
		switch (searchDto.SortBy)
		{
			case "ReleaseDate":
				query = searchDto.SortDescending ? query.OrderByDescending(mi => mi.FirstAirDate) : query.OrderBy(mi => mi.FirstAirDate);
				break;
			case "Name":
				query = searchDto.SortDescending ? query.OrderByDescending(mi => mi.Name) : query.OrderBy(mi => mi.Name);
				break;
			default:
				// Default sorting by release date
				query = query.OrderByDescending(mi => mi.FirstAirDate);
				break;
		}

		// Pagination
		var totalItems = await query.CountAsync();
		var items = await query
			.Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
			.Take(searchDto.PageSize)
			.Select(mi => _mapper.Map<MediaItemDto>(mi))
			.ToListAsync();

		return new CollectionResult<MediaItemDto>
		{
			Data = items,
			Count = totalItems
		};
	}
}
