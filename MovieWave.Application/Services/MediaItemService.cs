using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieWave.Application.Resources;
using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Interfaces.Validations;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class MediaItemService : IMediaItemService
{
	private readonly IBaseRepository<MediaItem> _mediaItemRepository;
	private readonly ITagService _tagService;
	private readonly ICountryService _countryService;
	private readonly IStudioService _studioService;
	private readonly IPersonService _personService;
	private readonly ILogger _logger;
	private readonly IMediaItemValidator _mediaItemValidator;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public MediaItemService(IBaseRepository<MediaItem> mediaItemRepository, ILogger logger,
		IMediaItemValidator mediaItemValidator, IMapper mapper, IUnitOfWork unitOfWork, ITagService tagService,
		ICountryService countryService, IStudioService studioService, IPersonService personService)
	{
		_mediaItemRepository = mediaItemRepository;
		_logger = logger;
		_mediaItemValidator = mediaItemValidator;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_tagService = tagService;
		_countryService = countryService;
		_studioService = studioService;
		_personService = personService;
	}

	public async Task<CollectionResult<MediaItemDto>> GetMediaItemsAsync(int pageNumber = 1, int pageSize = 10)
	{
		var query = _mediaItemRepository.GetAll()
			.Include(x => x.MediaItemType)
			.Include(x => x.Status)
			.Include(x => x.RestrictedRating)
			.Include(x => x.Episodes)
			.Include(x => x.Seasons)
			.Include(x => x.Attachments)
			.Include(x => x.Reviews)
			.Include(x => x.Notifications)
			.Include(x => x.Comments)
			.Include(x => x.Countries)
			.Include(x => x.Studios)
			.Include(x => x.Tags)
			.Include(x => x.MediaItemPeople)
			.ThenInclude(mp => mp.Person); ;

		var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

		if (!items.Any())
		{
			_logger.Warning(ErrorMessage.MediaItemsNotFound);
			return new CollectionResult<MediaItemDto>
			{
				ErrorMessage = ErrorMessage.MediaItemsNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemsNotFound
			};
		}

		var mediaItems = items.Select(x => _mapper.Map<MediaItemDto>(x)).ToList();

		return new CollectionResult<MediaItemDto>
		{
			Data = mediaItems,
			Count = totalItems
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
			.Include(x => x.Seasons)
			.Include(x => x.Attachments)
			.Include(x => x.Reviews)
			.Include(x => x.Notifications)
			.Include(x => x.Comments)
			.Include(x => x.Countries)
			.Include(x => x.Studios)
			.Include(x => x.Tags)
			.Include(x => x.MediaItemPeople)
			.ThenInclude(mp => mp.Person)
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

	public async Task<CollectionResult<MediaItemDto>> GetAllMoviesAsync(int pageNumber = 1, int pageSize = 10)
	{
		return await GetMediaItemsByTypeAsync(MediaItemName.Film, pageNumber, pageSize);
	}

	public async Task<CollectionResult<MediaItemDto>> GetAllSeriesAsync(int pageNumber = 1, int pageSize = 10)
	{
		return await GetMediaItemsByTypeAsync(MediaItemName.Series, pageNumber, pageSize);
	}

	private async Task<CollectionResult<MediaItemDto>> GetMediaItemsByTypeAsync(MediaItemName mediaType, int pageNumber, int pageSize)
	{
		var query = _mediaItemRepository.GetAll()
			.Include(x => x.MediaItemType)
			.Include(x => x.Status)
			.Include(x => x.RestrictedRating)
			.Include(x => x.Episodes)
			.Include(x => x.Seasons)
			.Include(x => x.Attachments)
			.Include(x => x.Reviews)
			.Include(x => x.Notifications)
			.Include(x => x.Comments)
			.Include(x => x.Countries)
			.Include(x => x.Studios)
			.Include(x => x.Tags)
			.Include(x => x.MediaItemPeople)
			.ThenInclude(mp => mp.Person); ;

		var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

		if (!items.Any())
		{
			return new CollectionResult<MediaItemDto>
			{
				ErrorMessage = ErrorMessage.MediaItemsNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemsNotFound
			};
		}

		var mediaItems = _mapper.Map<List<MediaItemDto>>(items);
		return new CollectionResult<MediaItemDto> { Data = mediaItems, Count = totalItems };
	}

	public async Task<CollectionResult<MediaItemByTagDto>> GetMoviesByTagAsync(long tagId, int pageNumber = 1, int pageSize = 10)
	{
		return await GetMediaItemsByTagAndTypeAsync(tagId, mediaTypeId: 1, pageNumber, pageSize);
	}

	public async Task<CollectionResult<MediaItemByTagDto>> GetSeriesByTagAsync(long tagId, int pageNumber = 1, int pageSize = 10)
	{
		return await GetMediaItemsByTagAndTypeAsync(tagId, mediaTypeId: 2, pageNumber, pageSize);
	}

	private async Task<CollectionResult<MediaItemByTagDto>> GetMediaItemsByTagAndTypeAsync(long tagId, int mediaTypeId, int pageNumber, int pageSize)
	{
		var query = _mediaItemRepository.GetAll()
			.Include(mi => mi.Tags)
			.Include(mi => mi.Attachments)
			.Include(mi => mi.SeoAddition)
			.Where(mi => mi.Tags.Any(t => t.Id == tagId) && mi.MediaItemTypeId == mediaTypeId);

		var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

		if (!items.Any())
		{
			_logger.Warning($"No media items found for TagId: {tagId} and MediaTypeId: {mediaTypeId}");
			return new CollectionResult<MediaItemByTagDto>
			{
				ErrorMessage = ErrorMessage.MediaItemsNotFound,
				ErrorCode = (int)ErrorCodes.MediaItemsNotFound
			};
		}

		var mediaItems = items.Select(mi => new MediaItemByTagDto
		{
			MediaItemId = mi.Id,
			Thumbnail = mi.Attachments.FirstOrDefault(a => a.AttachmentType == AttachmentType.Thumbnail)?.AttachmentUrl,
			Name = mi.Name,
			ReleaseYear = mi.FirstAirDate?.Year,
			Tags = mi.Tags.Select(t => new TagNameDto
			{
				Id = t.Id,
				Name = t.Name
			}).ToList(),
			SeoAddition = _mapper.Map<SeoAdditionInputDto>(mi.SeoAddition)
		}).ToList();

		return new CollectionResult<MediaItemByTagDto>
		{
			Data = mediaItems,
			Count = totalItems
		};
	}

	public async Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto)
	{
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var existingMediaItem = await _mediaItemRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Name == dto.Name);

			var validationResult = _mediaItemValidator.CreateValidator(existingMediaItem);

			if (!validationResult.IsSuccess)
			{
				return new BaseResult<MediaItemDto>
				{
					ErrorMessage = validationResult.ErrorMessage,
					ErrorCode = validationResult.ErrorCode
				};
			}

			var mediaItem = _mapper.Map<MediaItem>(dto);

			// Tags
			if (dto.TagIds != null && dto.TagIds.Any())
			{
				var tagsResult = await _tagService.GetTagsByIdsAsync(dto.TagIds);

				if (!tagsResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = tagsResult.ErrorMessage,
						ErrorCode = tagsResult.ErrorCode
					};
				}

				mediaItem.Tags = tagsResult.Data.ToList();
			}

			// Countries
			if (dto.CountryIds != null && dto.CountryIds.Any())
			{
				var countriesResult = await _countryService.GetCountriesByIdsAsync(dto.CountryIds);

				if (!countriesResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = countriesResult.ErrorMessage,
						ErrorCode = countriesResult.ErrorCode
					};
				}

				mediaItem.Countries = countriesResult.Data.ToList();
			}

			// Studio
			if (dto.StudioIds != null && dto.StudioIds.Any())
			{
				var studiosResult = await _studioService.GetStudioByIdsAsync(dto.StudioIds);

				if (!studiosResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = studiosResult.ErrorMessage,
						ErrorCode = studiosResult.ErrorCode
					};
				}

				mediaItem.Studios = studiosResult.Data.ToList();
			}

			// Person
			if (dto.PersonRoles != null && dto.PersonRoles.Any())
			{
				var personIds = dto.PersonRoles.Select(pr => pr.PersonId).ToList();
				var personsResult = await _personService.GetPersonsByIdsAsync(personIds);

				if (!personsResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = personsResult.ErrorMessage,
						ErrorCode = personsResult.ErrorCode
					};
				}

				mediaItem.MediaItemPeople = dto.PersonRoles
					.GroupBy(pr => new { pr.PersonId })
					.Select(g => g.First())
					.Select(pr => new MediaItemPerson
				{
					MediaItem = mediaItem,
					PersonId = pr.PersonId,
					PersonRole = (PersonRole)pr.Role
				}).ToList();
			}

			await _mediaItemRepository.CreateAsync(mediaItem);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<MediaItemDto>(mediaItem);

			return new BaseResult<MediaItemDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Помилка при створенні MediaItem: {Message}", ex.Message);

			if (transaction != null && transaction.GetDbTransaction().Connection != null)
			{
				await transaction.RollbackAsync();
			}

			return new BaseResult<MediaItemDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
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
		using var transaction = await _unitOfWork.BeginTransactionAsync();

		try
		{
			var mediaItem = await _mediaItemRepository.GetAll()
				.Include(x => x.Tags)
				.Include(x => x.Countries)
				.Include(x => x.Studios)
				.Include(x => x.MediaItemPeople)
				.FirstOrDefaultAsync(x => x.Id == dto.Id);

			if (mediaItem == null)
			{
				return new BaseResult<MediaItemDto>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			_mapper.Map(dto, mediaItem);

			// Update Tags
			if (dto.TagIds != null)
			{
				var tagsResult = await _tagService.GetTagsByIdsAsync(dto.TagIds);
				if (!tagsResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = tagsResult.ErrorMessage,
						ErrorCode = tagsResult.ErrorCode
					};
				}
				mediaItem.Tags = tagsResult.Data.ToList();
			}

			// Update Countries
			if (dto.CountryIds != null)
			{
				var countriesResult = await _countryService.GetCountriesByIdsAsync(dto.CountryIds);
				if (!countriesResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = countriesResult.ErrorMessage,
						ErrorCode = countriesResult.ErrorCode
					};
				}
				mediaItem.Countries = countriesResult.Data.ToList();
			}

			// Update Studios
			if (dto.StudioIds != null)
			{
				var studiosResult = await _studioService.GetStudioByIdsAsync(dto.StudioIds);
				if (!studiosResult.IsSuccess)
				{
					return new BaseResult<MediaItemDto>
					{
						ErrorMessage = studiosResult.ErrorMessage,
						ErrorCode = studiosResult.ErrorCode
					};
				}
				mediaItem.Studios = studiosResult.Data.ToList();
			}

			// Update People with Roles
			if (dto.PersonRoles != null)
			{
				mediaItem.MediaItemPeople.Clear();
				foreach (var pr in dto.PersonRoles)
				{
					mediaItem.MediaItemPeople.Add(new MediaItemPerson
					{
						MediaItemId = mediaItem.Id,
						PersonId = pr.PersonId,
						PersonRole = (PersonRole)pr.Role
					});
				}
			}

			_mediaItemRepository.Update(mediaItem);
			await _unitOfWork.SaveChangesAsync();
			await transaction.CommitAsync();

			var resultDto = _mapper.Map<MediaItemDto>(mediaItem);

			return new BaseResult<MediaItemDto> { Data = resultDto };
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			_logger.Error(ex, "Error updating MediaItem: {Message}", ex.Message);

			return new BaseResult<MediaItemDto>
			{
				ErrorMessage = ErrorMessage.InternalServerError,
				ErrorCode = (int)ErrorCodes.InternalServerError
			};
		}
	}

	public async Task<CollectionResult<MediaItemDto>> SearchMediaItemsAsync(MediaItemSearchDto searchDto, int pageNumber = 1, int pageSize = 10)
	{
		var query = _mediaItemRepository.GetAll()
			.Include(mi => mi.Tags)
			.Include(mi => mi.Status)
			.Include(mi => mi.MediaItemType)
			.Include(mi => mi.Countries)    // Додаємо Include для країн
			.Include(mi => mi.Studios)      // Додаємо Include для студій
			.AsQueryable();

		// Search by name and description
		if (!string.IsNullOrEmpty(searchDto.Query))
		{
			// Якщо у вас постгрес і ви використовуєте full-text search, все ок. Якщо ні - замініть на простий Contains.
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

		// Filter by media type (film or series)
		if (searchDto.MediaTypeId.HasValue)
		{
			query = query.Where(mi => mi.MediaItemTypeId == searchDto.MediaTypeId.Value);
		}

		// Filter by year
		if (searchDto.Year.HasValue)
		{
			// Переконуємося, що FirstAirDate не null
			query = query.Where(mi => mi.FirstAirDate.HasValue && mi.FirstAirDate.Value.Year == searchDto.Year.Value);
		}

		// Filter by countries
		if (searchDto.CountryIds != null && searchDto.CountryIds.Any())
		{
			query = query.Where(mi => mi.Countries.Any(c => searchDto.CountryIds.Contains(c.Id)));
		}

		// Filter by studios
		if (searchDto.StudioIds != null && searchDto.StudioIds.Any())
		{
			query = query.Where(mi => mi.Studios.Any(s => searchDto.StudioIds.Contains(s.Id)));
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
				// Default sorting by release date desc
				query = query.OrderByDescending(mi => mi.FirstAirDate);
				break;
		}

		var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

		var mediaItems = items.Select(mi => _mapper.Map<MediaItemDto>(mi)).ToList();

		return new CollectionResult<MediaItemDto>
		{
			Data = mediaItems,
			Count = totalItems
		};
	}

	public bool CheckSlugExists(string slug)
	{
		return _mediaItemRepository.GetAll().Any(m => m.SeoAddition.Slug == slug);
	}

	public async Task<bool> ExistsByNameAndYearAsync(string name, int? year)
	{
		var query = _mediaItemRepository.GetAll().Where(mi => mi.Name == name);
		if (year.HasValue)
			query = query.Where(mi => mi.FirstAirDate.HasValue && mi.FirstAirDate.Value.Year == year.Value);

		return await query.AnyAsync();
	}
}
