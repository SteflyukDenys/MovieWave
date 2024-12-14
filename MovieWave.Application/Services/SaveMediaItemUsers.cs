using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services
{
	public class SaveMediaItemUsers : ISaveMediaItemUsers
	{
		private readonly IBaseRepository<UserMediaItemList> _watchHistoryRepository;
		private readonly IBaseRepository<User> _userRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public SaveMediaItemUsers(
			IBaseRepository<UserMediaItemList> watchHistoryRepository,
			IBaseRepository<MediaItem> mediaItemRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			ILogger logger,
			IBaseRepository<User> userRepository)
		{
			_watchHistoryRepository = watchHistoryRepository;
			_mediaItemRepository = mediaItemRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<BaseResult<List<MediaItemDto>>> GetWatchHistoryAsync(Guid userId, int pageNumber = 1, int pageSize = 10)
		{
			var query = _watchHistoryRepository.GetAll()
				.Where(w => w.UserId == userId && w.ListTypeId == ListType.Watching)
				.Include(w => w.MediaItem)
					.ThenInclude(mi => mi.MediaItemType)
				.Include(w => w.MediaItem.Status)
				.Include(w => w.MediaItem.RestrictedRating)
				.Include(w => w.MediaItem.Episodes)
				.Include(w => w.MediaItem.Seasons)
				.Include(w => w.MediaItem.Attachments)
				.Include(w => w.MediaItem.Reviews)
				.Include(w => w.MediaItem.Notifications)
				.Include(w => w.MediaItem.Comments)
				.Include(w => w.MediaItem.Countries)
				.Include(w => w.MediaItem.Studios)
				.Include(w => w.MediaItem.Tags)
				.Include(w => w.MediaItem.People);

			var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

			if (!items.Any())
			{
				_logger.Warning($"No watch history found for UserId: {userId}");
				return new BaseResult<List<MediaItemDto>>
				{
					ErrorMessage = ErrorMessage.WatchHistoryNotFound,
					ErrorCode = (int)ErrorCodes.WatchHistoryNotFound
				};
			}

			var mediaItems = items.Select(w => _mapper.Map<MediaItemDto>(w.MediaItem)).ToList();

			return new BaseResult<List<MediaItemDto>>
			{
				Data = mediaItems
			};
		}

		public async Task<BaseResult<bool>> AddToWatchHistoryAsync(Guid userId, Guid mediaItemId)
		{
			var mediaItem = await _mediaItemRepository.GetAll().FirstOrDefaultAsync(mi => mi.Id == mediaItemId);
			if (mediaItem == null)
			{
				return new BaseResult<bool>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			var existingEntry = await _watchHistoryRepository.GetAll()
				.FirstOrDefaultAsync(w => w.UserId == userId && w.MediaItemId == mediaItemId && w.ListTypeId == ListType.Watching);

			if (existingEntry != null)
			{
				return new BaseResult<bool>
				{
					Data = true,
				};
			}

			var watchHistory = new UserMediaItemList
			{
				UserId = userId,
				MediaItemId = mediaItemId,
				ListTypeId = ListType.Watching
			};

			await _watchHistoryRepository.CreateAsync(watchHistory);
			await _unitOfWork.SaveChangesAsync();

			return new BaseResult<bool>
			{
				Data = true
			};
		}

		public async Task<BaseResult<bool>> AddToFavoritesAsync(Guid userId, Guid mediaItemId)
		{
			var existingEntry = await _watchHistoryRepository.GetAll()
				.FirstOrDefaultAsync(w => w.UserId == userId && w.MediaItemId == mediaItemId && w.ListTypeId == ListType.Favorites);

			if (existingEntry != null)
			{
				return new BaseResult<bool>
				{
					Data = true
				};
			}

			var favorite = new UserMediaItemList
			{
				UserId = userId,
				MediaItemId = mediaItemId,
				ListTypeId = ListType.Favorites
			};

			await _watchHistoryRepository.CreateAsync(favorite);
			await _unitOfWork.SaveChangesAsync();

			return new BaseResult<bool>
			{
				Data = true
			};
		}

		public async Task<BaseResult<List<MediaItemDto>>> GetFavoritesAsync(Guid userId, int pageNumber = 1, int pageSize = 10)
		{
			var query = _mediaItemRepository.GetAll()
				.Where(mi => mi.UserMediaItemLists.Any(w => w.UserId == userId && w.ListTypeId == ListType.Favorites))
				.Include(mi => mi.MediaItemType)
				.Include(mi => mi.Status)
				.Include(mi => mi.RestrictedRating)
				.Include(mi => mi.Episodes)
				.Include(mi => mi.Seasons)
				.Include(mi => mi.Attachments)
				.Include(mi => mi.Reviews)
				.Include(mi => mi.Notifications)
				.Include(mi => mi.Comments)
				.Include(mi => mi.Countries)
				.Include(mi => mi.Studios)
				.Include(mi => mi.Tags)
				.Include(mi => mi.People);

			var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

			if (!items.Any())
			{
				_logger.Warning($"No favorites found for UserId: {userId}");
				return new BaseResult<List<MediaItemDto>>
				{
					ErrorMessage = ErrorMessage.FavoritesNotFound,
					ErrorCode = (int)ErrorCodes.FavoritesNotFound
				};
			}

			var mediaItems = items.Select(mi => _mapper.Map<MediaItemDto>(mi)).ToList();

			return new BaseResult<List<MediaItemDto>>
			{
				Data = mediaItems
			};
		}

		public async Task<BaseResult<bool>> AddToWantToWatchAsync(Guid userId, Guid mediaItemId)
		{
			var existingEntry = await _watchHistoryRepository.GetAll()
				.FirstOrDefaultAsync(w => w.UserId == userId && w.MediaItemId == mediaItemId && w.ListTypeId == ListType.PlanToWatch);

			if (existingEntry != null)
			{
				return new BaseResult<bool>
				{
					Data = true
				};
			}

			var wantToWatch = new UserMediaItemList
			{
				UserId = userId,
				MediaItemId = mediaItemId,
				ListTypeId = ListType.PlanToWatch
			};

			await _watchHistoryRepository.CreateAsync(wantToWatch);
			await _unitOfWork.SaveChangesAsync();

			return new BaseResult<bool>
			{
				Data = true
			};
		}

		public async Task<BaseResult<List<MediaItemDto>>> GetWantToWatchAsync(Guid userId, int pageNumber = 1, int pageSize = 10)
		{
			var query = _mediaItemRepository.GetAll()
				.Where(mi => mi.UserMediaItemLists.Any(w => w.UserId == userId && w.ListTypeId == ListType.PlanToWatch))
				.Include(mi => mi.MediaItemType)
				.Include(mi => mi.Status)
				.Include(mi => mi.RestrictedRating)
				.Include(mi => mi.Episodes)
				.Include(mi => mi.Seasons)
				.Include(mi => mi.Attachments)
				.Include(mi => mi.Reviews)
				.Include(mi => mi.Notifications)
				.Include(mi => mi.Comments)
				.Include(mi => mi.Countries)
				.Include(mi => mi.Studios)
				.Include(mi => mi.Tags)
				.Include(mi => mi.People);

			var (items, totalItems) = await PaginationHelper.PaginateAsync(query, pageNumber, pageSize);

			if (!items.Any())
			{
				_logger.Warning($"No 'Want to Watch' items found for UserId: {userId}");
				return new BaseResult<List<MediaItemDto>>
				{
					ErrorMessage = ErrorMessage.WantToWatchNotFound,
					ErrorCode = (int)ErrorCodes.WantToWatchNotFound
				};
			}

			var mediaItems = items.Select(mi => _mapper.Map<MediaItemDto>(mi)).ToList();

			return new BaseResult<List<MediaItemDto>>
			{
				Data = mediaItems
			};
		}
	}
}
